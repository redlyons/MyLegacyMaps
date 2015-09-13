using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using MyLegacyMaps;
using MyLegacyMaps.Managers;
using MyLegacyMaps.Models.Account;
using MLM.Persistence.Interfaces;
using MLM.Logging;
using MyLegacyMaps.Models;
using MyLegacyMaps.Extensions;
using MyLegacyMaps.Classes.Paypal;


namespace MyLegacyMaps.Controllers
{
    public class ProfileController : Controller
    {
       
        private readonly ILogger log = null;
        private ApplicationUserManager _userManager;
        private readonly IPaymentsRepository _paymentRepository;


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ProfileController(IPaymentsRepository paymentRepository, ILogger logger)
        {
            _paymentRepository = paymentRepository;
            log = logger;
        }

        public string PaypalSubmitUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["PayPalSubmitUrl"];
            }
        }
       
        // GET: Profile/Private/5
        [HttpGet]
        public async Task<ActionResult> Private()
        {
            string userId = String.Empty;
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated )
                {
                    return new HttpUnauthorizedResult();
                }

                userId = HttpContext.User.Identity.GetUserId();
                if (String.IsNullOrEmpty(userId))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var applicationUser = await UserManager.FindByIdAsync(userId);
                if (applicationUser == null)
                {
                    return HttpNotFound();
                }               

                return View(applicationUser);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in ProfileController GET Private");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Profile/Credits/5
        [HttpGet]
        public async Task<ActionResult> Credits()
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                string id = HttpContext.User.Identity.GetUserId();
                if (String.IsNullOrEmpty(id))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var applicationUser = await UserManager.FindByIdAsync(id);
                if (applicationUser == null)
                {
                    return HttpNotFound();
                }

                var payments = await _paymentRepository.GetPaymentsAsync(id);
                if (payments.IsSuccess())
                {
                    ViewBag.Payments = payments.Item.ToViewModel();
                    ViewBag.HasPayments = payments.Item.Count > 0;
                }
                else ViewBag.HasPayments = false;

                ViewBag.PaypalSubmitUrl = PaypalSubmitUrl;
                return View(applicationUser);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in ProfileController GET Credits");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Profile/Credits/5
        [HttpGet]
        public async Task<ActionResult> ThankYou()
        {
            string authenticatedUserId = HttpContext.User.Identity.GetUserId();
            if (String.IsNullOrEmpty(authenticatedUserId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var authToken = System.Configuration.ConfigurationManager.AppSettings["PDTToken"].ToString();
                
                //read in txn token from querystring
                var txToken = Request.QueryString.Get("tx");
                var paymentResp = await _paymentRepository.GetPaymentAsync(txToken);
                if(paymentResp.IsSuccess())
                {
                    var tx = paymentResp.Item;
                    var details = new StringBuilder();
                    details.AppendLine(
                       string.Format("Thank you {0} {1} ({2}) for your payment of {3} {4}!",
                       tx.PayerFirstName, tx.PayerLastName,
                       tx.PayerEmail, tx.GrossTotal, tx.Currency));

                    details.AppendLine(System.Environment.NewLine);
                    details.AppendLine(string.Format("Your account has been credited with {0} token{1}. Happy pinning!",
                        tx.Tokens.ToString(), (tx.Tokens > 1) ? "s" : ""));

                    ViewBag.Message = details.ToString();
                    return View();
                }
                
                var query = string.Format("cmd=_notify-synch&tx={0}&at={1}",
                                      txToken, authToken);

                // Create the request back
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(PaypalSubmitUrl);

                // Set values for the request back
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = query.Length;

                // Write the request back IPN strings
                StreamWriter stOut = new StreamWriter(req.GetRequestStream(),
                                         System.Text.Encoding.ASCII);
                stOut.Write(query);
                stOut.Close();

                // Do the request to PayPal and get the response
                StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
                var strResponse = stIn.ReadToEnd();
                stIn.Close();
                              
                var message = new StringBuilder();
                var pdt = PDTHolder.Parse(strResponse);

                //Log Payment Record
                var payment = pdt.ToPaymentModel(authenticatedUserId);
                var addPaymentResp = await _paymentRepository.AddPaymentAsync(payment);
                if(!addPaymentResp.IsSuccess())
                {
                    log.Error("Failed to Add Payment record, transaction detials = {0}", pdt.TransactionDetails);
                }

                // If response was SUCCESS, parse response string and output details                
                if (strResponse.StartsWith("SUCCESS"))
                {
                    //Update user Tokens
                    var addUserTokens = true;
                    var user = await UserManager.FindByIdAsync(authenticatedUserId);
                    if (user == null)
                    {
                        addUserTokens = false;
                    }
                    else
                    {
                        user.Credits = user.Credits + payment.Tokens;
                        var updateUserResp = await UserManager.UpdateAsync(user);
                        if(!updateUserResp.Succeeded)
                        {
                            addUserTokens = false;
                        }
                    }
                    
                    message.AppendLine(
                        string.Format("Thank you {0} {1} ({2}) for your payment of {3} {4}!",
                        pdt.PayerFirstName, pdt.PayerLastName,
                        pdt.PayerEmail, pdt.GrossTotal, pdt.Currency));

                    message.AppendLine(System.Environment.NewLine);
                    message.AppendLine(string.Format("Your account has been credited with {0} token{1}. Happy pinning!",
                        pdt.Tokens.ToString(), (pdt.Tokens > 1) ? "s" : ""));


                    if(!addUserTokens)
                    {
                        message.AppendLine("While your transaction has succeeded, there was a problem crediting your account. We're terribly sorry, please contact us at mylegacymaps@gmail.com.");
                    }                  
                }
                else
                {
                    message.AppendLine("Oooops, something went wrong, please check your PayPal account for details. If you have questions concerns please contact us at mylegacymaps@gmail.com");
                }

                ViewBag.Message = message.ToString();
                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in ProfileController GET Credits");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult> TotalCredits()
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                string id = HttpContext.User.Identity.GetUserId();
                if (String.IsNullOrEmpty(id))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var applicationUser = await UserManager.FindByIdAsync(id);
                if (applicationUser == null)
                {
                    return HttpNotFound();
                }

                return Json(applicationUser.Credits, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in ProfileController GET TotalCredits");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

     

        // GET: Maps/Edit/5
        public async Task<ActionResult> Edit()
        {
            string userId = String.Empty;
            try
            {

                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                userId = HttpContext.User.Identity.GetUserId();
                if (String.IsNullOrEmpty(userId))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var applicationUser = await UserManager.FindByIdAsync(userId);
                if (applicationUser == null)
                {
                    return HttpNotFound();
                }                
                return View(applicationUser);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in ProfileController GET Edit");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: Maps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include ="Id,Email,DisplayName")] ApplicationUser updatedUser)
        {
            string userId = String.Empty;

            if(!ModelState.IsValid)
            {
                return View(updatedUser);
            }

            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }
                userId = HttpContext.User.Identity.GetUserId();
                if (String.IsNullOrEmpty(userId))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if(userId != updatedUser.Id)
                {
                    return new HttpUnauthorizedResult();
                }

               
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }



                var user = await UserManager.FindByIdAsync(updatedUser.Id);

                if (user == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if(user.Email != updatedUser.Email)
                {
                    //check that email is not already in use
                    var existingUser = await UserManager.FindByEmailAsync(updatedUser.Email);
                    if(existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Email Address Not Available");
                        return View(updatedUser);
                    }
                }

                user.DisplayName = (String.IsNullOrEmpty(updatedUser.DisplayName)) 
                    ? String.Empty : updatedUser.DisplayName;

                user.ProfileImageUrl = (String.IsNullOrEmpty(updatedUser.ProfileImageUrl))
                   ? String.Empty : updatedUser.ProfileImageUrl;
                
                user.EmailPrevious = user.Email;
                user.Email = updatedUser.Email;
                user.UserName = updatedUser.Email;
                user.EmailConfirmed = (user.Email.ToLower() != user.EmailPrevious.ToLower());
                user.DateModified = System.DateTime.Now;
                await UserManager.UpdateAsync(user);

                return RedirectToAction("Private");
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in ProfileController POST Edit ");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
              

        protected override void Dispose(bool disposing)
        {
           base.Dispose(disposing);
        }
    }
}
