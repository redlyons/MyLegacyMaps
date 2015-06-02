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
using MyLegacyMaps.DataAccess;
using MyLegacyMaps.DataAccess.Resources;
using MyLegacyMaps.Models;

namespace MyLegacyMaps.Controllers
{
    public class FlagsController : Controller
    {
        private readonly MyLegacyMapsContext db = new MyLegacyMapsContext();

        // GET: Flags
        //public async Task<ActionResult> Index()
        //{
        //    if (!HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        return new HttpUnauthorizedResult();
        //    }
        //    return View(await db.Flags.ToListAsync());
        //}

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
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var flag = await db.Flags.FindAsync(id);
                if (flag == null)
                {
                    return HttpNotFound();
                }
                var adoptedMap = await db.AdoptedMaps.FindAsync(flag.AdoptedMapId);
                if (adoptedMap == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
                if (adoptedMap.UserId != User.Identity.GetUserId())
                {
                    return new HttpUnauthorizedResult();
                }
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
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

                flag.CreatedDate = flag.ModifiedDate = DateTime.Now;
                db.Flags.Add(flag);
                await db.SaveChangesAsync();
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Flags/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (!HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        return new HttpUnauthorizedResult();
        //    }
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Flag flag = await db.Flags.FindAsync(id);
        //    if (flag == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(flag);
        //}

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

                var adoptedMap = await db.AdoptedMaps.FindAsync(flag.AdoptedMapId);
                if (adoptedMap == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
                if (adoptedMap.UserId != User.Identity.GetUserId())
                {
                    return new HttpUnauthorizedResult();
                }
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                flag.ModifiedDate = DateTime.Now;
                db.Entry(flag).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(flag, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
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
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }
                                
                Flag flag = await db.Flags.FindAsync(id);
                if (flag != null)
                {
                    db.Flags.Remove(flag);
                    await db.SaveChangesAsync();
                }
                
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch(Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
