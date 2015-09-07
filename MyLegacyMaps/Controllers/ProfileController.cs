using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;

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


namespace MyLegacyMaps.Controllers
{
    public class ProfileController : Controller
    {
       
        private readonly ILogger log = null;
        private ApplicationUserManager _userManager;


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

        public ProfileController(ILogger logger)
        {
            log = logger;
        }
       
        // GET: Profile/Private/5
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

                return View(applicationUser);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in ProfileController GET Credits");
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

                user.DisplayName = (String.IsNullOrEmpty(updatedUser.DisplayName)) ? String.Empty : updatedUser.DisplayName;
                user.Email = updatedUser.Email;
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
