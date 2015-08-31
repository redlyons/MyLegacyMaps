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
using MyLegacyMaps.Managers;
using MyLegacyMaps.Models.Account;
using MLM.Logging;
using MLM.Persistence.Interfaces;
using MLM.Persistence;

using MyLegacyMaps.Models;
using MyLegacyMaps.Extensions;
using MyLegacyMaps.Classes.Cookies;

namespace MyLegacyMaps.Controllers
{
    [Authorize(Roles = "mapManager")]
    public class AdminController : Controller
    {
        private IPhotoService photoStorage = null;
        private IMapsRepository mapsRepository = null;
        private IPartnerLogosRepository logosRepository = null;
        private ILogger log = null;
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

        public AdminController(IMapsRepository mapsResource,
                               IPartnerLogosRepository logosResource, 
                               IPhotoService photoService, 
                               ILogger logger)
        {
            mapsRepository = mapsResource;
            logosRepository = logosResource;
            photoStorage = photoService;
            log = logger;            
        }

       
        // GET: Admin
        [Authorize(Roles = "mapManager")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get List of Maps to Manage
        /// </summary>
        [Authorize(Roles = "mapManager")]
        [HttpGet]
        public ActionResult UsersManage(int? page)
        {
            try
            {
                // Get Maps by map type id
                var users = UserManager.Users.ToList<ApplicationUser>();
                var usersViewModel = users.OrderBy(m => m.Email);

                int pageSize = 20;
                int pageNumber = (page ?? 1);

                return View(usersViewModel.ToPagedList(pageNumber, pageSize));

            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController GET MapsManage");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Maps/Edit/5
        [Authorize(Roles = "mapManager")]
        public async Task<ActionResult> UserEdit(string id)
        {
            try
            {

                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                if (String.IsNullOrEmpty(id))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "missing id parameter");
                }

                var user = await UserManager.FindByIdAsync(id);
                return View(user);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController UserEdit id = {0} ",
                  (!String.IsNullOrEmpty(id)) ? id: "null");

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: Maps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "mapManager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserEdit([Bind(Include =
            "Id,Email,EmailConfirmed,DisplayName,Credits")] ApplicationUser updatedUser)
        {

            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var user = await UserManager.FindByIdAsync(updatedUser.Id);

                if(user == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                user.DisplayName = (String.IsNullOrEmpty(updatedUser.DisplayName)) ? String.Empty : updatedUser.DisplayName;
                user.Email = updatedUser.Email;
                user.EmailConfirmed = updatedUser.EmailConfirmed;
                user.Credits = (updatedUser.Credits <= 0)? 0 : updatedUser.Credits;          


                await UserManager.UpdateAsync(user);

                

                return RedirectToAction("UsersManage");
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController POST MapEdit UserId = {0} ",
                 updatedUser.Id);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get List of Maps to Manage
        /// </summary>
        [Authorize(Roles = "mapManager")]
        [HttpGet]
        public async Task<ActionResult> MapsManage()
        {
            try
            {
                // Get Maps by map type id
                var resp = await mapsRepository.AdminGetMapsAsync();
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                //View
                var mapsViewModel = resp.Item.ToViewModel(true);
                return View(mapsViewModel.OrderBy(m => m.Name));

            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController GET MapsManage");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
                

        // GET: Maps/Create
        [Authorize(Roles = "mapManager")]
        public async Task<ActionResult> MapCreate()
        {
            var mapViewModel = new Map()
            {
                IsActive = true,
                OrientationTypeId = 1,
                OrientationType = new OrientationType { OrientationTypeId = 1, Name = "Horizontal" }
            };
                

            ViewBag.MapTypes = await GetMapTypes();
            ViewBag.AspectRatios = await GetAspectRatios();
            return View(mapViewModel);
        }

        // POST: Maps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "mapManager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MapCreate([Bind(Include = "MapId,AspectRationId,Name,Description,FileName,MapTypeId,OrientationTypeId,IsActive")] Map map,
            HttpPostedFileBase photo, HttpPostedFileBase thumb, FormCollection values)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if(photo != null)
                {                    
                    map.ImageUrl = await photoStorage.UploadPhotoAsync(photo, PhotoType.MapMainImage);
                    map.FileName = photo.FileName;
                }

                if(thumb != null)
                {
                    map.ThumbUrl = await photoStorage.UploadPhotoAsync(thumb, PhotoType.MapThumb);

                }

                List<int> mapTypeIds = new List<int>();
                if (!String.IsNullOrEmpty(values["selectedMapTypes"]))
                {
                    var selectedMapTypes = values["selectedMapTypes"].Split(new char[] { ',' });
                    for (int i = 0; i < selectedMapTypes.Count(); i++)
                    {
                        int typeId = 0;
                        if (Int32.TryParse(selectedMapTypes[i], out typeId))
                        {
                            mapTypeIds.Add(typeId);
                        }
                    }
                }

                int chosenAspectRation = 0;
                if (values["aspectRatio"] != null && Int32.TryParse(values["aspectRatio"], out chosenAspectRation))
                {
                    map.AspectRatioId = chosenAspectRation;
                }

                map.DateCreated = DateTime.Now;
                map.DateModified = DateTime.Now;
                map.ModifiedBy = HttpContext.User.Identity.Name;
                var resp = await mapsRepository.AdminCreateMapAsync(map.ToDomainModel());
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                if(mapTypeIds.Count > 0)
                {
                    resp = await mapsRepository.AdminSaveMapTypesAsync(resp.Item.MapId, mapTypeIds);
                    if (!resp.IsSuccess())
                    {
                        return new HttpStatusCodeResult(resp.HttpStatusCode);
                    }
                }

                return RedirectToAction("MapsManage");
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController POST MapCreate");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }


        }

        // GET: Maps/Edit/5
        [Authorize(Roles = "mapManager")]
        public async Task<ActionResult> MapEdit(int? id)
        {
            try
            {

                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "missing id parameter");
                }

                var resp = await mapsRepository.AdminGetMapAsync((int)id.Value);
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                ViewBag.MapTypes =  await GetMapTypes();
                ViewBag.AspectRatios = await GetAspectRatios();
                return View(resp.Item.ToViewModel());
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController GET MapEdit id = {0} ",
                  (id.HasValue) ? id.Value.ToString() : "null");

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: Maps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "mapManager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MapEdit([Bind(Include =
            "MapId,AspectRatioId,Name,Description,FileName,OrientationTypeId,IsActive,ImageUrl,ThumbUrl")] Map map,        
            HttpPostedFileBase photo, HttpPostedFileBase thumb, FormCollection values)
        {

            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if(photo != null)
                {
                    map.ImageUrl = await photoStorage.UploadPhotoAsync(photo, PhotoType.MapMainImage);
                    map.FileName = photo.FileName;
                }

                if (thumb != null)
                {
                    map.ThumbUrl = await photoStorage.UploadPhotoAsync(thumb, PhotoType.MapThumb);
                }

                List<int> updatedMapTypeIds = new List<int>();
                if (!String.IsNullOrEmpty(values["selectedMapTypes"]))
                { 
                    var selectedMapTypes = values["selectedMapTypes"].Split(new char [] {','});               
                    for (int i = 0; i < selectedMapTypes.Count(); i++)
                    {
                        int typeId = 0;
                        if (Int32.TryParse(selectedMapTypes[i], out typeId))
                        {
                            updatedMapTypeIds.Add(typeId);
                        }
                    }
                }

                int chosenAspectRation = 0;
                if (values["aspectRatio"] != null && Int32.TryParse(values["aspectRatio"], out chosenAspectRation))
                {
                    map.AspectRatioId = chosenAspectRation;
                }

                map.DateModified = DateTime.Now;
                map.ModifiedBy = HttpContext.User.Identity.Name;
                map.OrientationType = null;//avoid mismatch error if OrientationTypeId property has changed
                var resp = await mapsRepository.AdminSaveMapAsync(map.ToDomainModel());
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                resp = await mapsRepository.AdminSaveMapTypesAsync(map.MapId, updatedMapTypeIds);
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                return RedirectToAction("MapsManage");
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController POST MapEdit MapId = {0} ",
                 map.MapId);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get List of Map Types to Manage
        /// </summary>
        [Authorize(Roles = "mapManager")]
        [HttpGet]
        public async Task<ActionResult> MapTypesManage()
        {
            try
            {
                // Get Maps by map type id
                var resp = await mapsRepository.AdminGetMapTypesAsync();
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                //View
                var mapTypesViewModel = resp.Item.ToViewModel(true);
                return View(mapTypesViewModel.OrderBy(m => m.Name));

            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController GET MapTypesManage");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: MapTypes/Edit/5
        [Authorize(Roles = "mapManager")]
        public async Task<ActionResult> MapTypeEdit(int? id)
        {
            try
            {

                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                if (!id.HasValue || (int)id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "missing id parameter");
                }

                var resp = await mapsRepository.AdminGetMapTypeAsync((int)id.Value);
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }
              
                return View(resp.Item.ToViewModel());
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController GET MapTypeEdit id = {0} ",
                  (id.HasValue) ? id.Value.ToString() : "null");

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Authorize(Roles = "mapManager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MapTypeEdit([Bind(Include = "MapTypeId, Name, IsActive")] MapType mapType)
        {

            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }


                var resp = await mapsRepository.AdminSaveMapTypeAsync(mapType.ToDomainModel());
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                return RedirectToAction("MapTypesManage");
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController POST MapTypeEdit MapTypeId = {0} ",
                 mapType.MapTypeId);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }


        // GET: Maps/Create
        [Authorize(Roles = "mapManager")]
        public ActionResult MapTypeCreate()
        {           
            return View();
        }

        // POST: Maps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "mapManager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MapTypeCreate([Bind(Include = "MapTypeId,Name,IsActive")] MapType mapType)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }


                var resp = await mapsRepository.AdminCreateMapTypeAsync(mapType.ToDomainModel());

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController POST MapCreate");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }


        }

        private async Task<List<MapType>> GetMapTypes()
        {
            var resp = await mapsRepository.GetMapTypesAsync();
            if (!resp.IsSuccess())
            {
                return new List<MapType>();
            }
            var viewModel = resp.Item.ToViewModel(true).ToList<MapType>().OrderBy(m => m.Name);
            return viewModel.ToList();
        }

        private async Task<List<AspectRatio>> GetAspectRatios()
        {
            var resp = await mapsRepository.GetAspectRatiosAsync();
            if (!resp.IsSuccess())
            {
                return new List<AspectRatio>();
            }
            var viewModel = resp.Item.ToViewModel().ToList<AspectRatio>().OrderBy(m => m.Name);
            return viewModel.ToList();
        }


        /// <summary>
        /// Get List of Partner Logos to Manage
        /// </summary>
        [Authorize(Roles = "mapManager")]
        [HttpGet]
        public async Task<ActionResult> PartnerLogosManage()
        {
            try
            {
                // Get Maps by map type id
                var resp = await logosRepository.AdminGetPartnerLogosAsync();
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                //View
                var viewModel = resp.Item.ToViewModel();
                return View(viewModel.OrderBy(m => m.Name));

            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController GET PartnerLogosManage");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: MapTypes/Edit/5
        [Authorize(Roles = "mapManager")]
        public async Task<ActionResult> PartnerLogoEdit(int? id)
        {
            try
            {

                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                if (!id.HasValue || (int)id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "missing id parameter");
                }

                var resp = await logosRepository.AdminGetPartnerLogoAsync((int)id.Value);
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                return View(resp.Item.ToViewModel());
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController GET PartnerLogoEdit id = {0} ",
                  (id.HasValue) ? id.Value.ToString() : "null");

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Authorize(Roles = "mapManager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PartnerLogoEdit([Bind(Include = "PartnerLogoId, Name, IsActive, ImageUrl,Height,Width")] PartnerLogo logo,
            HttpPostedFileBase thumb)
        {

            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (thumb != null)
                {
                    logo.ImageUrl = await photoStorage.UploadPhotoAsync(thumb, PhotoType.PartnerLogo);                   
                }              

                var resp = await logosRepository.AdminSavePartnerLogoAsync(logo.ToDomainModel());
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                return RedirectToAction("PartnerLogosManage");
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController POST PartnerLogoEdit PartnerLogoId = {0} ",
                 logo.PartnerLogoId);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }


        // GET: Maps/Create
        [Authorize(Roles = "mapManager")]
        public ActionResult PartnerLogoCreate()
        {
            return View(new PartnerLogo { IsActive = true });
        }

        // POST: Maps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "mapManager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PartnerLogoCreate([Bind(Include = "PartnerLogoId,Name,IsActive,ImageUrl,Height,Width")] PartnerLogo logo,
            HttpPostedFileBase thumb)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (thumb != null)
                {
                    logo.ImageUrl = await photoStorage.UploadPhotoAsync(thumb, PhotoType.PartnerLogo);
                }

                var resp = await logosRepository.AdminCreatePartnerLogoAsync(logo.ToDomainModel());

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdminController POST PartnerLogoCreate");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }


        }

        
    }
}