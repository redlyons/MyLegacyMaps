using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyLegacyMaps.DataAccess;
using MyLegacyMaps.Models;
using Microsoft.AspNet.Identity;
using MLM.Logging;


namespace MyLegacyMaps.Controllers
{
    public class AdoptedMapsController : Controller
    {
        private MyLegacyMapsContext db = new MyLegacyMapsContext();
        private ILogger log = null;

        public AdoptedMapsController(ILogger logger)
        {
            log = logger;
        }

        // GET: AdoptedMaps
        public async Task<ActionResult> Index()
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }
                string userId = User.Identity.GetUserId();
                var result = db.AdoptedMaps.Where(a => a.UserId == userId);

                if (result == null)
                {
                    return HttpNotFound();
                }
                var adoptedMaps = await result.ToListAsync();
                return View(adoptedMaps.OrderBy(m => m.Name));
            }
            catch(Exception ex)
            {
                log.Error(ex, "Error in AdoptedMapsController GET Index");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: AdoptedMaps/Details/5
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
                AdoptedMap adoptedMap = await db.AdoptedMaps.FindAsync(id);
                //var map = await db.Maps.FindAsync(adoptedMap.MapId);
                if (adoptedMap == null)// || map == null)
                {
                    return HttpNotFound();
                }

                //adoptedMap.Map = map;
                return View(adoptedMap);
            }
            catch(Exception ex)
            {
                log.Error(ex, "Error in AdoptedMapsController GET Details id = {0} ", 
                    (id.HasValue)? id.Value.ToString() : "null");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: AdoptedMaps/Create
        //public ActionResult Create()
        //{
        //    if(!HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        return new HttpUnauthorizedResult();
        //    }
        //    return View();
        //}

        // POST: AdoptedMaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,MapId,Name,ShareStatusTypeId")] AdoptedMap adoptedMap)
        {
            var userId = "";
            try
            { 
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }

                userId = User.Identity.GetUserId();
                if(String.IsNullOrWhiteSpace(userId))
                {
                    return new HttpUnauthorizedResult();
                }

                if (adoptedMap == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "null post data");
                }
               
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                adoptedMap.UserId = userId;
                adoptedMap.ShareStatusTypeId = 1; //default to private
                db.AdoptedMaps.Add(adoptedMap);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
                //return View(adoptedMap);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdoptedMapsController POST Create userId = {0} mapId = {1}",
                    userId, adoptedMap.MapId);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: AdoptedMaps/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return new HttpUnauthorizedResult();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdoptedMap adoptedMap = await db.AdoptedMaps.FindAsync(id);
            if (adoptedMap == null)
            {
                return HttpNotFound();
            }
            return View(adoptedMap);
        }

        // POST: AdoptedMaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AdoptedMapId,UserId,MapId,Name,ShareStatus")] AdoptedMap adoptedMap)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return new HttpUnauthorizedResult();
            }
            if (ModelState.IsValid)
            {
                db.Entry(adoptedMap).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(adoptedMap);
        }

        // GET: AdoptedMaps/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return new HttpUnauthorizedResult();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdoptedMap adoptedMap = await db.AdoptedMaps.FindAsync(id);
            if (adoptedMap == null)
            {
                return HttpNotFound();
            }
            return View(adoptedMap);
        }

        // POST: AdoptedMaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AdoptedMap adoptedMap = await db.AdoptedMaps.FindAsync(id);
            db.AdoptedMaps.Remove(adoptedMap);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
