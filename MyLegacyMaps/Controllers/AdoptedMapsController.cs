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


namespace MyLegacyMaps.Controllers
{
    public class AdoptedMapsController : Controller
    {
        private MyLegacyMapsContext db = new MyLegacyMapsContext();

        // GET: AdoptedMaps
        public async Task<ActionResult> Index()
        {


            string userId = User.Identity.GetUserId();
            var result = db.AdoptedMaps.Where(a => a.UserId == userId);

            if(result == null)
            {
                return HttpNotFound();
            }
            var adoptedMaps = await result.ToListAsync();
            return View(adoptedMaps.OrderBy(m => m.Name));
        }

        // GET: AdoptedMaps/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdoptedMap adoptedMap = await db.AdoptedMaps.FindAsync(id);           
            var map = await db.Maps.FindAsync(adoptedMap.MapId);
            if (adoptedMap == null || map == null)
            {
                return HttpNotFound();
            }

            adoptedMap.Map = map;
            return View(adoptedMap);
        }

        // GET: AdoptedMaps/Create
        public ActionResult Create()
        {
            if(!HttpContext.User.Identity.IsAuthenticated)
            {
                return new HttpUnauthorizedResult();
            }
            return View();
        }

        // POST: AdoptedMaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,MapId,Name,ShareStatus")] AdoptedMap adoptedMap)
        {            
            string userId = User.Identity.GetUserId();
            int mapId = 0;
            var data = HttpContext.Request.Form;

            if(data != null && !String.IsNullOrEmpty(userId) && Int32.TryParse(data["mapId"], out mapId) && mapId > 0)
            {
                adoptedMap = new AdoptedMap
                { 
                    MapId = mapId, 
                    UserId = userId, 
                    Name = data["mapName"],
                    ShareStatus = 1
                };
                db.AdoptedMaps.Add(adoptedMap);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            //if (ModelState.IsValid)
            //{
                
               
            //}

            return View(adoptedMap);
        }

        // GET: AdoptedMaps/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
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
