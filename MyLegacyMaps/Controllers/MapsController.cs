using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyLegacyMaps.Models;
using MyLegacyMaps.Extensions;
using MLM.Logging;
using MLM.Persistence.Interfaces;

namespace MyLegacyMaps.Controllers
{
    public class MapsController : Controller
    {
        private IMapsRepository mapsRepository = null;
        private const string MAPTYPEID_COOKIE = "mlm_map_filterid";
        private ILogger log = null;
        
        public MapsController(IMapsRepository repositiory, ILogger logger)
        {
            mapsRepository = repositiory;
            log = logger;
        }

        // GET: Maps
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            try
            {
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

                var resp = await mapsRepository.GetMapsAsync(mapTypeId);
                var mapTypesOptions = await GetMapTypeOptions(mapTypeId);
                if(!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

               
                var mapsViewModel = resp.Item.ToViewModel();
                ViewBag.mapTypes = mapTypesOptions;
                return View(mapsViewModel.OrderBy(m => m.Name));

            }
            catch(Exception ex)
            {
                log.Error(ex, "Error in MapsController GET Index");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Filter Maps by MapTypeId
        /// </summary>
        /// <param name="values">ddlMapTypeId</param>
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

                var resp = await mapsRepository.GetMapsAsync(mapTypeId);
                var mapTypesOptions = await GetMapTypeOptions(mapTypeId);

                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                
                var mapsViewModel = resp.Item.ToViewModel();
                ViewBag.mapTypes = mapTypesOptions;
                return View(mapsViewModel.OrderBy(m => m.Name));                      
            }
            catch(Exception ex)
            {
                log.Error(ex, String.Format("Error in MapsController POST Index mapTypeId = {0}",
                    mapTypeId));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    

        public async Task<SelectList> GetMapTypeOptions(int selectedMapTypeId)
        {
           // IEnumerable<SelectListItem> types = (from m in db.MapTypes where m.IsActive == true select m).AsEnumerable().OrderBy(m=>m.Name).Select(m => new SelectListItem() { Text = m.Name, Value = m.MapTypeId.ToString() });

           
            var resp = await mapsRepository.GetMapTypesAsync();
            if(!resp.IsSuccess())
            {
                return new SelectList(new List<MapType>(), "Value", "Text"); //empty list
            }
            var mapTypes = resp.Item.ToViewModel();

            IEnumerable<SelectListItem> types = mapTypes.OrderBy(m => m.Name).Select(m => 
                new SelectListItem() { Text = m.Name, Value = m.MapTypeId.ToString() });
            
            var mapTypeOptions =  (selectedMapTypeId > 0)
                    ? new SelectList(types, "Value", "Text", selectedMapTypeId)
                    : new SelectList(types, "Value", "Text");

            return mapTypeOptions;
        }

        // GET: Maps/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            try
            { 
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "missing id parameter");
                }

                var resp = await mapsRepository.FindMapByIdAsync((int)id.Value);
                if(!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }
                return View(resp.Item.ToViewModel());
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
    }
}
