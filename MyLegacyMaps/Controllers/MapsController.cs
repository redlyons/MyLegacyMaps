using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MLM.Logging;
using MLM.Persistence.Interfaces;
using MyLegacyMaps.Models;
using MyLegacyMaps.Extensions;
using MyLegacyMaps.Classes.Cookies;

namespace MyLegacyMaps.Controllers
{
    public class MapsController : Controller
    {
        private IMapsRepository mapsRepository = null;        
        private ILogger log = null;
        private ICookieHelper cookies = null;
        
        public MapsController(IMapsRepository repositiory, ILogger logger, ICookieHelper cookieHelper)
        {
            mapsRepository = repositiory;
            log = logger;
            cookies = cookieHelper;
        }

        /// <summary>
        /// Get Public List of Active Maps
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            try
            {
                //My Type ddl options - get selected value from cookie
                int mapTypeId = cookies.GetCookie<int>(Constants.COOKIE_MAPTYPEID, 
                    this.ControllerContext.HttpContext);

                ViewBag.mapTypes = await GetMapTypeOptions(mapTypeId);

                //Get Maps by map type id
                var resp = await mapsRepository.GetMapsAsync(mapTypeId);
                if(!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }
               
                //View
                var mapsViewModel = resp.Item.ToViewModel();               
                return View(mapsViewModel.OrderBy(m => m.Name));

            }
            catch(Exception ex)
            {
                log.Error(ex, "Error in MapsController GET Index");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get Public List of maps filtered by MapTypeId.
        /// </summary>
        /// <param name="values">ddlMapTypeId</param>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult>Index(FormCollection values)
        {
            int mapTypeId = 0;
            try
            {     
                //Read in May Type drop down value and save in cookie
                Int32.TryParse(values["ddlMapTypeId"], out mapTypeId);
                cookies.SetCookie(Constants.COOKIE_MAPTYPEID, 
                    mapTypeId.ToString(), this.ControllerContext.HttpContext);

                //Get Map Types
                ViewBag.mapTypes = await GetMapTypeOptions(mapTypeId);
               
                //Get Maps by Map TypeID
                var resp = await mapsRepository.GetMapsAsync(mapTypeId);
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                //View
                var mapsViewModel = resp.Item.ToViewModel();
                return View(mapsViewModel.OrderBy(m => m.Name));                      
            }
            catch(Exception ex)
            {
                log.Error(ex, String.Format("Error in MapsController POST Index mapTypeId = {0}",
                    mapTypeId));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    

        private async Task<SelectList> GetMapTypeOptions(int selectedMapTypeId)
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

        #region MapManager Endpoints
        /// <summary>
        /// Get List of Maps to Manage
        /// </summary>
        [Authorize(Roles = "mapManager")]
        [HttpGet]
        public async Task<ActionResult> Manage()
        {
            try
            {                
                //My Type ddl options - get selected value from cookie
                int mapTypeId = cookies.GetCookie<int>(Constants.COOKIE_MAPTYPEID,
                    this.ControllerContext.HttpContext);

                ViewBag.mapTypes = await GetMapTypeOptions(mapTypeId);

                // Get Maps by map type id
                var resp = await mapsRepository.GetMapsAsync(mapTypeId);               
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
                log.Error(ex, "Error in MapsController GET Index");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Filter List of Maps by MapTypeId
        /// </summary>
        /// <param name="values"></param>
        [Authorize(Roles = "mapManager")]
        [HttpPost]
        public async Task<ActionResult> Manage(FormCollection values)
        {
            int mapTypeId = 0;
            try
            {
                //Read in May Type drop down value and save in cookie
                Int32.TryParse(values["ddlMapTypeId"], out mapTypeId);
                cookies.SetCookie(Constants.COOKIE_MAPTYPEID,
                    mapTypeId.ToString(), this.ControllerContext.HttpContext);

                //Get Map Types
                ViewBag.mapTypes = await GetMapTypeOptions(mapTypeId);

                //Get Maps by Map TypeID
                var resp = await mapsRepository.GetMapsAsync(mapTypeId);
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
                log.Error(ex, String.Format("Error in MapsController POST Index mapTypeId = {0}",
                    mapTypeId));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Maps/Create
        [Authorize(Roles = "mapManager")]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Maps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "mapManager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MapId,Name,FileName,Orientation,IsActive")] Map map)
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

                map.DateCreated = DateTime.Now;
                var resp = await mapsRepository.CreateMapAsync(map.ToDomainModel());
                
                return RedirectToAction("Index");                
            }
            catch(Exception ex)
            {
                log.Error(ex, "Error in MapsController POST Create");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            
        }

        // GET: Maps/Edit/5
        [Authorize(Roles = "mapManager")]
        public async Task<ActionResult> Edit(int? id)
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

                var resp = await mapsRepository.FindMapByIdAsync((int)id.Value);
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }
                return View(resp.Item.ToViewModel());
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in MapsController GET Edit id = {0} ",
                  (id.HasValue) ? id.Value.ToString() : "null");

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }        
        }

        // POST: Maps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MapId,Name,FileName,Orientation,IsActive")] Map map)
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


                var resp = await mapsRepository.SaveMapAsync(map.ToDomainModel());
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }
                   
                return RedirectToAction("Manage");
                
                
            }
            catch(Exception ex)
            {
                log.Error(ex, "Error in MapsController POST Edit MapId = {0} ",
                 map.MapId);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Maps/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
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
        //[Authorize(Roles = "mapManager")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Map map = await db.Maps.FindAsync(id);
        //    db.Maps.Remove(map);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}   
        #endregion
    }
}
