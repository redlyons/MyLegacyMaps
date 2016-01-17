using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using PagedList;
using MLM.Logging;
using MLM.Persistence.Interfaces;
using MyLegacyMaps.Models;
using MyLegacyMaps.Extensions;
using MyLegacyMaps.Classes;
using MyLegacyMaps.Managers;

namespace MyLegacyMaps.Controllers
{
    public class PublishedMapsController : Controller
    {
        private IAdoptedMapsRepository adoptedMapsRepository = null;
        private ApplicationUserManager _userManager;
        private IPartnerLogosRepository logosRepository = null;
        private ILogger log = null;
       
        public PublishedMapsController(IAdoptedMapsRepository adoptedMapsResource, IPartnerLogosRepository logosResource, ILogger logger)
        {
            adoptedMapsRepository = adoptedMapsResource;
            logosRepository = logosResource;
            log = logger;
        }

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

        // GET: PublishedMaps
        [AllowAnonymous]
        public async Task<ActionResult> Index(string u, int? page)
        {
            // string userId = String.Empty;
            try
            {
                if (String.IsNullOrEmpty(u))
                {
                    if (HttpContext.User.Identity.IsAuthenticated)
                    {
                        u = HttpContext.User.Identity.GetUserId();
                    }                    
                }
                
                if(!String.IsNullOrEmpty(u))
                {
                    var user = UserManager.FindById(u);
                    if (user != null)
                    {
                        ViewBag.DisplayName = user.DisplayName;
                    }
                }

                var resp = await adoptedMapsRepository.GetPublicAdoptedMapsByUserIdAsync(u);
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                int pageNumber = (page ?? 1);
                var mapsViewModel = resp.Item.ToViewModel().OrderBy(m => m.Name);
                return View(mapsViewModel.ToPagedList(pageNumber, Constants.PAGE_SIZE));
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in PublishedMapsController GET PublishedMaps UserId = {0}", u));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: PublishedMaps/Map/5
        [AllowAnonymous]
        public async Task<ActionResult> Map(int? id)
        {
            try
            {
                if (!id.HasValue || (int)id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var resp = await adoptedMapsRepository.GetAdoptedMapByIdAsync((int)id.Value);
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }
                if (resp.Item.ShareStatusTypeId != (int)Enums.ShareStatusType.Public)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }
                return View(resp.Item.ToViewModel());
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdoptedMapsController GET SharedMap id = {0} ",
                    (id.HasValue) ? id.Value.ToString() : "null");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}