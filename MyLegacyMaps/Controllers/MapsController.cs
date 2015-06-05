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
using MLM.Logging;

namespace MyLegacyMaps.Controllers
{
    public class MapsController : Controller
    {
        private MyLegacyMapsContext db = new MyLegacyMapsContext();
        private const string MAPTYPEID_COOKIE = "mlm_map_filterid";
        private ILogger log = null;


        public MapsController(ILogger logger)
        {
            log = logger;
        }

        // GET: Maps
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            try
            {
                var mapList = await db.Maps.ToListAsync();
                var cookieVal = (this.ControllerContext.HttpContext.Request.Cookies[MAPTYPEID_COOKIE] != null)
                    ? this.ControllerContext.HttpContext.Request.Cookies[MAPTYPEID_COOKIE].Value
                    : String.Empty;

                int mapTypeId = 0;
                if (!String.IsNullOrEmpty(cookieVal))
                {
                    if (!Int32.TryParse(cookieVal, out mapTypeId))
                    {
                        mapTypeId = 0;
                    }
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
            catch(Exception ex)
            {
                log.Error(ex, "Error in MapsController GET Index");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult>Index(FormCollection values)
        {
            int mapTypeId = 0;

            try
            { 
               
                if (!String.IsNullOrWhiteSpace(values["ddlMapTypeId"]))
                {
                    Int32.TryParse(values["ddlMapTypeId"], out mapTypeId);
                }

                //save selection state in cookie
                HttpCookie cookie = new HttpCookie(MAPTYPEID_COOKIE, mapTypeId.ToString());
                cookie.HttpOnly = true;
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                //get map type drop down options
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
            catch(Exception ex)
            {
                log.Error(ex, String.Format("Error in MapsController POST Index mapTypeId = {0}",
                    mapTypeId));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    

        public SelectList getMapTypes(int selectedMapTypeId)
        {
            IEnumerable<SelectListItem> types = (from m in db.MapTypes where m.IsActive == true select m).AsEnumerable().OrderBy(m=>m.Name).Select(m => new SelectListItem() { Text = m.Name, Value = m.MapTypeId.ToString() });
            return (selectedMapTypeId > 0)
                    ? new SelectList(types, "Value", "Text", selectedMapTypeId)
                    : new SelectList(types, "Value", "Text");
        }

        // GET: Maps/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            try
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
            catch (Exception ex)
            {
                log.Error(ex, "Error in MapsController GET Details id = {0} ",
                  (id.HasValue) ? id.Value.ToString() : "null");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
           
        }

        // GET: Maps/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Maps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "MapId,Name,FileName,Orientation,IsActive")] Map map)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        db.Maps.Add(map);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(map);
        //}

        // GET: Maps/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Map map = await db.Maps.FindAsync(id);
        //    if (map == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(map);
        //}

        // POST: Maps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "MapId,Name,FileName,Orientation,IsActive")] Map map)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(map).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(map);
        //}

        // GET: Maps/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Map map = await db.Maps.FindAsync(id);
        //    if (map == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(map);
        //}

        // POST: Maps/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Map map = await db.Maps.FindAsync(id);
        //    db.Maps.Remove(map);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
