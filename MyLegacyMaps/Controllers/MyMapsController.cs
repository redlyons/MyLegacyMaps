﻿using System;
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
    public class MyMapsController : Controller
    {
        private IAdoptedMapsRepository adoptedMapsRepository = null;
        private ApplicationUserManager _userManager;
        private IPartnerLogosRepository logosRepository = null;
        private ILogger log = null;
       
        public MyMapsController(IAdoptedMapsRepository adoptedMapsResource, IPartnerLogosRepository logosResource, ILogger logger)
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

        // GET: AdoptedMaps
        public async Task<ActionResult> AdoptedMaps(int? page)
        {
            string userId = String.Empty;
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }
                userId = User.Identity.GetUserId();
                var resp = await adoptedMapsRepository.GetAdoptedMapsByUserIdAsync(userId);

                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                ViewBag.HasAdoptedMaps = resp.Item.Count > 0;

                var mapsViewModel = (resp.Item.Count > 0)
                    ? resp.Item.ToViewModel().OrderBy(m => m.Name)
                    : new List<MyLegacyMaps.Models.AdoptedMap>().OrderBy(m => m.Name);              

                int pageNumber = (page ?? 1);
                return View(mapsViewModel.ToPagedList(pageNumber, Constants.PAGE_SIZE));
            }
            catch(Exception ex)
            {
                log.Error(ex, String.Format("Error in AdoptedMapsController GET Index UserId = {0}", userId));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }      

        // GET: AdoptedMaps/Details/5
        public async Task<ActionResult> AdoptedMap(int? id)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }
                if (!id.HasValue || (int)id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var resp = await adoptedMapsRepository.GetAdoptedMapByIdAsync((int)id.Value);
                if(!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }
                return View(resp.Item.ToViewModel());
            }
            catch(Exception ex)
            {
                log.Error(ex, "Error in AdoptedMapsController GET Details id = {0} ", 
                    (id.HasValue)? id.Value.ToString() : "null");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: SharedMaps 
        [AllowAnonymous]
        public async Task<ActionResult> SharedMaps(int? page)
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
                
                var resp = await adoptedMapsRepository.GetPublicAdoptedMapsByUserIdAsync(userId);
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                ViewBag.HasSharedMaps = resp.Item.Count > 0;
                ViewBag.DisplayName = applicationUser.DisplayName;

                var mapsViewModel = (resp.Item.Count > 0)
                    ? resp.Item.ToViewModel().OrderBy(m => m.Name)
                    : new List<MyLegacyMaps.Models.AdoptedMap>().OrderBy(m => m.Name);
                            

                int pageNumber = (page ?? 1);                            
                return View(mapsViewModel.ToPagedList(pageNumber, Constants.PAGE_SIZE));
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in AdoptedMapsController GET Index UserId = {0}", userId));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: SharedMaps 
        [AllowAnonymous]
        public async Task<ActionResult> SharedMapsByUserId(string userId, int? page)
        {
            // string userId = String.Empty;
            try
            {
                if (String.IsNullOrEmpty(userId))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var resp = await adoptedMapsRepository.GetPublicAdoptedMapsByUserIdAsync(userId);
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
                log.Error(ex, String.Format("Error in AdoptedMapsController GET Index UserId = {0}", userId));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: AdoptedMaps/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> SharedMap(int? id)
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
                if(resp.Item.ShareStatusTypeId != (int)Enums.ShareStatusType.Public)
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

        // GET: AdoptedMaps/Details/5
        public async Task<ActionResult> RealEstateMap(int? id)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }
                if (!id.HasValue || (int)id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var resp = await adoptedMapsRepository.GetAdoptedMapByIdAsync((int)id.Value);
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                var respLogos = await logosRepository.GetPartnerLogosAsync();
                if (!respLogos.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                ViewBag.PartnerLogos = respLogos.Item.ToViewModel();
                return View(resp.Item.ToViewModel());
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdoptedMapsController GET RealEstateMap id = {0} ",
                    (id.HasValue) ? id.Value.ToString() : "null");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: MyMaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MapId, Name, Description")] AdoptedMap adoptedMap)
        {
            try
            { 
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }               

                adoptedMap.UserId = HttpContext.User.Identity.GetUserId();
                adoptedMap.DateCreated = DateTime.Now;
                adoptedMap.DateModified = DateTime.Now;
                adoptedMap.ModifiedBy = HttpContext.User.Identity.Name;
                adoptedMap.IsActive = true;

                //if (!ModelState.IsValid)
                //{
                //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //}

                var resp = await adoptedMapsRepository.CreateAdoptedMapAsync(adoptedMap.ToDomainModel());

                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                var mapViewModel = resp.Item.ToViewModel();

                if (mapViewModel.Map.IsRealEstateMap())
                {
                    return RedirectToAction("RealEstateMap", new { id = resp.Item.AdoptedMapId });
                }
                else
                {
                    return RedirectToAction("AdoptedMap", new { id = resp.Item.AdoptedMapId });
                }
             
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdoptedMapsController POST Create userId = {0} mapId = {1}",
                    adoptedMap.UserId, adoptedMap.MapId);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: AdoptedMaps/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }
                if (!id.HasValue || (int)id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var resp = await adoptedMapsRepository.GetAdoptedMapByIdAsync((int)id.Value);
              
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                if (resp.Item.UserId != HttpContext.User.Identity.GetUserId())
                {
                    return new HttpUnauthorizedResult();
                }

                ViewBag.ShareTypes = await GetShareTypeOptions();
                var viewModel = resp.Item.ToViewModel();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in AdoptedMapsController GET Edit Userid = {0}, id = {1}",
                    HttpContext.User.Identity.GetUserId(), (id.HasValue)? id.Value.ToString():"null"));

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: AdoptedMaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include =
            "AdoptedMapId, MapId, Name, Description, IsActive, ShareStatusTypeId, DateCreated, UserId")] AdoptedMap adoptedMap)
        {
            try
            { 
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }
                if (!ModelState.IsValid)
                {
                    return View(adoptedMap);
                }
                               
                adoptedMap.IsActive = true;
                adoptedMap.DateModified = DateTime.Now;
                adoptedMap.ModifiedBy = HttpContext.User.Identity.Name;
                adoptedMap.ShareStatusType = null; //avoid mismatch error if ShareStatusTypeId as changed.
                var saveResp = await adoptedMapsRepository.SaveAdoptedMapAsync(adoptedMap.ToDomainModel());
                if (!saveResp.IsSuccess())
                {
                    return new HttpStatusCodeResult(saveResp.HttpStatusCode);
                }

                var viewModel = saveResp.Item.ToViewModel();
                if (viewModel.Map.IsRealEstateMap())
                {
                    return RedirectToAction("RealEstateMap", new { id = adoptedMap.AdoptedMapId });
                }
                else
                {
                    return RedirectToAction("AdoptedMap", new { id = adoptedMap.AdoptedMapId });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in AdoptedMapsController POST Edit Userid = {0}, AdoptedMapId = {1}, Name = {2}",
                   HttpContext.User.Identity.GetUserId(), adoptedMap.AdoptedMapId, adoptedMap.Name));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: AdoptedMaps/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {

            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }
                if (!id.HasValue || (int)id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var resp = await adoptedMapsRepository.GetAdoptedMapByIdAsync((int)id);
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                return View(resp.Item.ToViewModel());
            }
            catch(Exception ex)
            {
                log.Error(ex, String.Format("Error in AdoptedMapsController GET Delete id = {0}",
                   (id.HasValue) ? id.Value.ToString() : "null"));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: AdoptedMaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }               
                if (!id.HasValue || (int)id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var getResp = await adoptedMapsRepository.GetAdoptedMapByIdAsync((int)id);
                if (!getResp.IsSuccess())
                {
                    return new HttpStatusCodeResult(getResp.HttpStatusCode);
                }

                if (getResp.Item.UserId != HttpContext.User.Identity.GetUserId())
                {
                    return new HttpUnauthorizedResult();
                }

                var deleteResp = await adoptedMapsRepository.DeleteAdoptedMapAsync(getResp.Item);
                
                return RedirectToAction("AdoptedMaps");

            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in AdoptedMapsController DELETE DeleteConfirmed UserId = {0}, id = {1}",
                    HttpContext.User.Identity.GetUserId(), (id.HasValue)? id.Value.ToString() : "null"));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }           
          
        }

        public async Task<List<ShareStatusType>> GetShareTypeOptions()
        {     

            var resp = await adoptedMapsRepository.GetShareTypesAsync();
            if (!resp.IsSuccess())
            {
                return new List<ShareStatusType>();
            }
            var viewModel = resp.Item.ToViewModel().ToList<ShareStatusType>().OrderBy(m => m.ShareStatusTypeId);
            return viewModel.ToList();
        }

        
    }
}
