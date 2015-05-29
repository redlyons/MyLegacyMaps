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

namespace MyLegacyMaps.Controllers
{
    public class MapsController : Controller
    {
        private MyLegacyMapsContext db = new MyLegacyMapsContext();

        // GET: Maps
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var mapList = await db.Maps.ToListAsync();
            //var mapTypes = await db.MapTypes.ToListAsync();
            ViewBag.mapTypes = getMapTypes(0);

            return View(mapList.OrderBy(m => m.Name));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult>Index(FormCollection values)
        {
            int mapTypeId = 0;
            if (!String.IsNullOrWhiteSpace(values["ddlMapTypeId"]))
            {
                Int32.TryParse(values["ddlMapTypeId"], out mapTypeId);
            }
            ViewBag.mapTypes = getMapTypes(mapTypeId);
            
            if (mapTypeId > 0)
            {
                return View(db.Maps.Where(m => m.MapTypeId == mapTypeId).OrderBy(m => m.Name));
            }
            else
            {
                var maps = await db.Maps.ToListAsync();
                return View(maps.OrderBy(m => m.Name));
            }              
        }
    

        public SelectList getMapTypes(int selectedMapTypeId)
        {
            IEnumerable<SelectListItem> types = (from m in db.MapTypes select m).AsEnumerable().OrderBy(m=>m.Name).Select(m => new SelectListItem() { Text = m.Name, Value = m.MapTypeId.ToString() });
            return (selectedMapTypeId > 0)
                    ? new SelectList(types, "Value", "Text", selectedMapTypeId)
                    : new SelectList(types, "Value", "Text");
        }

        // GET: Maps/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Map map = await db.Maps.FindAsync(id);
            if (map == null)
            {
                return HttpNotFound();
            }
            return View(map);
        }

        // GET: Maps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Maps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MapId,Name,FileName,Orientation,IsActive")] Map map)
        {
            if (ModelState.IsValid)
            {
                db.Maps.Add(map);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(map);
        }

        // GET: Maps/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Map map = await db.Maps.FindAsync(id);
            if (map == null)
            {
                return HttpNotFound();
            }
            return View(map);
        }

        // POST: Maps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MapId,Name,FileName,Orientation,IsActive")] Map map)
        {
            if (ModelState.IsValid)
            {
                db.Entry(map).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(map);
        }

        // GET: Maps/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Map map = await db.Maps.FindAsync(id);
            if (map == null)
            {
                return HttpNotFound();
            }
            return View(map);
        }

        // POST: Maps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Map map = await db.Maps.FindAsync(id);
            db.Maps.Remove(map);
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
