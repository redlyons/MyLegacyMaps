﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
        private ILogger log = null;

        public AdminController(IMapsRepository repositiory, IPhotoService photoService, ILogger logger)
        {
            mapsRepository = repositiory;
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
                var mapsViewModel = resp.Item.ToViewModel();
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
                    OrientationTypeId = 1,
                    OrientationType = new OrientationType
                    {
                        OrientationTypeId = 1,
                        Name = "Horizontal"
                    }
                };

            ViewBag.MapTypes = await GetMapTypes();
            return View(mapViewModel);
        }

        // POST: Maps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "mapManager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MapCreate([Bind(Include = "MapId,Name,FileName,MapTypeId,OrientationTypeId,IsActive")] Map map,
            HttpPostedFileBase photo, HttpPostedFileBase thumb)
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

                map.DateCreated = DateTime.Now;
                map.DateModified = DateTime.Now;
                map.ModifiedBy = HttpContext.User.Identity.Name;
                var resp = await mapsRepository.AdminCreateMapAsync(map.ToDomainModel());

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

                var mapViewModel = resp.Item.ToViewModel();              
                var mapTypes = await GetMapTypes();

                ViewBag.MapTypes = mapTypes;
                return View(mapViewModel);
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
        public async Task<ActionResult> MapEdit([Bind(Include = "MapId,Name,FileName,OrientationTypeId,IsActive")] Map map,        
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
                var selectedMapTypes = values["selectedMapTypes"].Split(new char [] {','});
                if(selectedMapTypes != null && selectedMapTypes.Count() > 0)
                {
                    var allMapTypes = await GetMapTypes();
                    map.MapTypes = new List<MapType>();
                    for (int i = 0; i < selectedMapTypes.Count(); i++)
                    {
                        int typeId = 0;
                        if (Int32.TryParse(selectedMapTypes[i], out typeId))
                        {
                            foreach (var type in allMapTypes)
                            {

                                if (type.MapTypeId == typeId)
                                {
                                    map.MapTypes.Add(type);
                                    break;
                                }
                            
                            }
                        }
                    }
                }

                map.DateModified = DateTime.Now;
                map.ModifiedBy = HttpContext.User.Identity.Name;
                var resp = await mapsRepository.AdminSaveMapAsync(map.ToDomainModel());
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
                var mapTypesViewModel = resp.Item.ToViewModel();
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
            var viewModel = resp.Item.ToViewModel().ToList<MapType>().OrderBy(m => m.Name);
            return viewModel.ToList();
        }
    }
}