using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MLM.Persistence.Interfaces;
using MLM.Logging;
using MyLegacyMaps.Models;
using MyLegacyMaps.Extensions;

namespace MyLegacyMaps.Controllers
{
    public class FlagsController : Controller
    {
        private readonly IFlagsRepository flagRepository = null;
        private readonly IAdoptedMapsRepository adoptedMapRepository = null;
        private readonly ILogger log = null;

        public FlagsController(IFlagsRepository flagsRepository, IAdoptedMapsRepository adoptedMapsRepository, ILogger logger)
        {
            flagRepository = flagsRepository;
            adoptedMapRepository = adoptedMapsRepository;
            log = logger;
        }

        
        // GET: Flags/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }
                if (!id.HasValue  || (int)id.Value <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }


                var respFlag = await flagRepository.FindFlagByIdAsync((int)id);
                if (!respFlag.IsSuccess())
                {
                    return new HttpStatusCodeResult(respFlag.HttpStatusCode);
                }


                var respAdoptedMap = await adoptedMapRepository.FindByAdoptedMapIdAsync(respFlag.Item.AdoptedMapId);
                if (!respAdoptedMap.IsSuccess())
                {
                    return new HttpStatusCodeResult(respAdoptedMap.HttpStatusCode);
                }

                if (respAdoptedMap.Item.UserId != User.Identity.GetUserId())
                {
                    return new HttpUnauthorizedResult();
                }
                return Json(respFlag.Item.ToViewModel(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex, "Error in FlagsController GET Details id = {0} ",
                   (id.HasValue) ? id.Value.ToString() : "null");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

       
        // POST: Flags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FlagTypeId,AdoptedMapId,Name,Xpos,Ypos,Date,Description,VideoUrl,")] Flag flag)
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
                
                flag.DateCreated = flag.DateModified = DateTime.Now;
                flag.ModifiedBy = HttpContext.User.Identity.Name;
                var resp = await flagRepository.AddFlagAsync(flag.ToDomainModel());
                if(!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                return Json(resp.Item.ToViewModel(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex, String.Format("Error in FlagsController POST Create Userid = {0} ",
                    HttpContext.User.Identity.GetUserId()));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }       

        // POST: Flags/Edit/5      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AdoptedMapId,FlagId,FlagTypeId,Name,Xpos,Ypos,Date,Description,VideoUrl")] Flag flag)
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

                var adoptedMapResp = await adoptedMapRepository.FindByAdoptedMapIdAsync(flag.AdoptedMapId);
                if (!adoptedMapResp.IsSuccess())
                {
                    return new HttpStatusCodeResult(adoptedMapResp.HttpStatusCode);
                }
                if (adoptedMapResp.Item.UserId != User.Identity.GetUserId())
                {
                    return new HttpUnauthorizedResult();
                }                
                
                flag.DateModified = DateTime.Now;
                flag.ModifiedBy = HttpContext.User.Identity.Name;
                var flagResp = await flagRepository.SaveFlagAsync(flag.ToDomainModel());
                if (!flagResp.IsSuccess())
                {
                    return new HttpStatusCodeResult(flagResp.HttpStatusCode);
                }
                return Json(flagResp.Item.ToViewModel(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(ex, String.Format("Error in FlagsController POST Edit Userid = {0} ",
                   HttpContext.User.Identity.GetUserId()));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Flags/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    try
        //    {
        //        if (!HttpContext.User.Identity.IsAuthenticated)
        //        {
        //            return new HttpUnauthorizedResult();
        //        }
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        Flag flag = await db.Flags.FindAsync(id);
        //        if (flag == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(flag);
        //    }
        //    catch (Exception)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        //    }
        //}

        // POST: Flags/Delete/5
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
                if (!id.HasValue || (int)id.Value <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }


                var respFlagGet = await flagRepository.FindFlagByIdAsync((int)id);
                if (!respFlagGet.IsSuccess())
                {
                    return new HttpStatusCodeResult(respFlagGet.HttpStatusCode);
                }
                var adoptedMapResp = await adoptedMapRepository.FindByAdoptedMapIdAsync(respFlagGet.Item.AdoptedMapId);
                if (!adoptedMapResp.IsSuccess())
                {
                    return new HttpStatusCodeResult(adoptedMapResp.HttpStatusCode);
                }
                if (adoptedMapResp.Item.UserId != User.Identity.GetUserId())
                {
                    return new HttpUnauthorizedResult();
                }


                var respFlagDel = await flagRepository.DeleteFlagAsync(respFlagGet.Item);
                if(!respFlagDel.IsSuccess())
                {
                    return new HttpStatusCodeResult(respFlagDel.HttpStatusCode);
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                log.Error(ex, String.Format("Error in FlagsController POST DeleteConfirmed Userid = {0} id = {1} ",
                    HttpContext.User.Identity.GetUserId(), id));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }       
    }
}
