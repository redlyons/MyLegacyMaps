using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
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
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get Public List of Active Maps
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> AdventureMaps(int? currentFilterId, int? page)
        {
            int mapTypeId = 0;
         
            try
            {
                //Read in Map Type drop down value and save in cookie
                if (currentFilterId.HasValue)
                {
                    mapTypeId = (int)currentFilterId.Value;
                    cookies.SetCookie(Constants.COOKIE_MAPTYPEID,
                        mapTypeId.ToString(), this.ControllerContext.HttpContext);
                }
                else
                {
                    //get selected value from cookie
                    mapTypeId = cookies.GetCookie<int>(Constants.COOKIE_MAPTYPEID,
                       this.ControllerContext.HttpContext);
                }

                var mapTypes = new List<MapType>();
                var respMapTypes = await mapsRepository.GetMapTypesAsync();
                var realEstateMapType = new MapType{ MapTypeId = 1, Name = "Real Estate", IsActive = true };
                if (respMapTypes.IsSuccess())
                {
                    mapTypes = respMapTypes.Item.ToViewModel(false);                   
                    
                }

                //Get Maps by map type id
                var resp = await mapsRepository.GetMapsAsync(mapTypeId);
                if(!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }
                            
                //View
                ViewBag.mapTypes = mapTypes;
                ViewBag.CurrentFilter = (mapTypeId > 0)
                   ? mapTypes.Find(mt => mt.MapTypeId == mapTypeId).Name
                   : String.Empty;

                int pageSize = 4;
                int pageNumber = (page ?? 1);
                var mapsViewModel = resp.Item.ToViewModel(false).OrderBy(m => m.Name);
                return View(mapsViewModel.ToPagedList(pageNumber, pageSize));

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
        public async Task<ActionResult> AdventureMaps(FormCollection values)
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

        /// <summary>
        /// Get Public List of Active Maps
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> RealEstateMaps(int? page)
        {
            int mapTypeId = 1;            
            try
            {               
                //Get Maps by map type id
                var resp = await mapsRepository.GetMapsAsync(mapTypeId);
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                //View
                int pageSize = 4;
                int pageNumber = (page ?? 1);
                var mapsViewModel = resp.Item.ToViewModel(true).OrderBy(m => m.Name);
                return View(mapsViewModel.ToPagedList(pageNumber, pageSize));

            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in MapsController GET Index");
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
            var mapTypes = resp.Item.ToViewModel(false);

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

                var resp = await mapsRepository.GetMapAsync((int)id.Value);
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

       
    }
}
