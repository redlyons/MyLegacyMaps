using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MLM.Logging;
using MLM.Persistence.Interfaces;
using MyLegacyMaps.Models;
using MyLegacyMaps.Extensions;


namespace MyLegacyMaps.Controllers
{
    public class AdoptedMapsController : Controller
    {
        private IAdoptedMapsRepository adoptedMapsRepository = null;
        private ILogger log = null;

        public AdoptedMapsController(IAdoptedMapsRepository repository, ILogger logger)
        {
            adoptedMapsRepository = repository;
            log = logger;
        }

        // GET: AdoptedMaps
        public async Task<ActionResult> Index()
        {
            string userId = String.Empty;
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }
                userId = User.Identity.GetUserId();
                var resp = await adoptedMapsRepository.GetAdoptedMapsByUserIdAsync(userId);

                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                var viewModel = resp.Item.ToViewModel();
                return View(viewModel.OrderBy(m => m.Name));
            }
            catch(Exception ex)
            {
                log.Error(ex, String.Format("Error in AdoptedMapsController GET Index UserId = {0}", userId));
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
                if (!id.HasValue || (int)id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var resp = await adoptedMapsRepository.FindByAdoptedMapIdAsync((int)id.Value);
                if(!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }
                return View(resp.Item.ToViewModel());
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

                adoptedMap.UserId = HttpContext.User.Identity.GetUserId();
                adoptedMap.ShareStatusTypeId = 1; //default to private
                adoptedMap.DateCreated = DateTime.Now;
                adoptedMap.DateModified = DateTime.Now;
                adoptedMap.ModifiedBy = HttpContext.User.Identity.Name;

                var resp = await adoptedMapsRepository.CreateAdoptedMapAsync(adoptedMap.ToDomainModel());

                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }
               
                return RedirectToAction("Index");
             
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error in AdoptedMapsController POST Create userId = {0} mapId = {1}",
                    adoptedMap.UserId, adoptedMap.MapId);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: AdoptedMaps/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }
                if (!id.HasValue || (int)id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var resp = await adoptedMapsRepository.FindByAdoptedMapIdAsync((int)id.Value);
              
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                if (resp.Item.UserId != HttpContext.User.Identity.GetUserId())
                {
                    return new HttpUnauthorizedResult();
                }

                var shareTypeOptions = await GetShareTypeOptions(resp.Item.ShareStatusTypeId);
                ViewBag.ShareTypes = shareTypeOptions;
                var viewModel = resp.Item.ToViewModel();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in AdoptedMapsController GET Edit Userid = {0}, id = {1}",
                    HttpContext.User.Identity.GetUserId(), (id.HasValue)? id.Value.ToString():"null"));

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: AdoptedMaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AdoptedMapId, Name, ShareStatusTypeId, ShareStatusType.Name")] AdoptedMap adoptedMap, FormCollection values)
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
                var resp = await adoptedMapsRepository.FindByAdoptedMapIdAsync(adoptedMap.AdoptedMapId);
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }
                if (resp.Item.UserId != HttpContext.User.Identity.GetUserId())
                {
                    return new HttpUnauthorizedResult();
                }

                var updatedMap = resp.Item;
                updatedMap.Name = adoptedMap.Name;
                updatedMap.DateModified = DateTime.Now;
                updatedMap.ModifiedBy = HttpContext.User.Identity.Name;

                var saveResp = await adoptedMapsRepository.SaveAdoptedMapAsync(updatedMap);
                if (!saveResp.IsSuccess())
                {
                    return new HttpStatusCodeResult(saveResp.HttpStatusCode);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in AdoptedMapsController POST Edit Userid = {0}, AdoptedMapId = {1}, Name = {2}",
                   HttpContext.User.Identity.GetUserId(), adoptedMap.AdoptedMapId, adoptedMap.Name));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: AdoptedMaps/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {

            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return new HttpUnauthorizedResult();
                }
                if (!id.HasValue || (int)id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var resp = await adoptedMapsRepository.FindByAdoptedMapIdAsync((int)id);
                if (!resp.IsSuccess())
                {
                    return new HttpStatusCodeResult(resp.HttpStatusCode);
                }

                return View(resp.Item.ToViewModel());
            }
            catch(Exception ex)
            {
                log.Error(ex, String.Format("Error in AdoptedMapsController GET Delete id = {0}",
                   (id.HasValue) ? id.Value.ToString() : "null"));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: AdoptedMaps/Delete/5
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
                if (!id.HasValue || (int)id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var getResp = await adoptedMapsRepository.FindByAdoptedMapIdAsync((int)id);
                if (!getResp.IsSuccess())
                {
                    return new HttpStatusCodeResult(getResp.HttpStatusCode);
                }

                if (getResp.Item.UserId != HttpContext.User.Identity.GetUserId())
                {
                    return new HttpUnauthorizedResult();
                }

                var deleteResp = await adoptedMapsRepository.DeleteAdoptedMapAsync(getResp.Item);
                
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in AdoptedMapsController DELETE DeleteConfirmed UserId = {0}, id = {1}",
                    HttpContext.User.Identity.GetUserId(), (id.HasValue)? id.Value.ToString() : "null"));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }           
          
        }

        public async Task<SelectList> GetShareTypeOptions(int selectedShareTypeId)
        {
            var resp = await adoptedMapsRepository.GetShareTypesAsync();
            if (!resp.IsSuccess())
            {
                return new SelectList(new List<MapType>(), "Value", "Text"); //empty list
            }
            var shareTypes = resp.Item.ToViewModel();

            IEnumerable<SelectListItem> types = shareTypes.OrderBy(m => m.Name).Select(m =>
                new SelectListItem() { Text = m.Name, Value = m.ShareStatusTypeId.ToString() });

            var mapTypeOptions = (selectedShareTypeId > 0)
                    ? new SelectList(types, "Value", "Text", selectedShareTypeId)
                    : new SelectList(types, "Value", "Text");

            return mapTypeOptions;
        }

        
    }
}
