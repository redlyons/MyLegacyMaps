using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MLM.Models;
using MLM.Persistence.Interfaces;
using MLM.Logging;

namespace MLM.Persistence
{
    public class AdoptedMapsRepository : IAdoptedMapsRepository, IDisposable
    {
        private MyLegacyMapsContext db = new MyLegacyMapsContext();
        private readonly ILogger log = null;

        public AdoptedMapsRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task<ResourceResponse<List<AdoptedMap>>> GetAdoptedMapsByUserIdAsync(string userId)
        {
            List<AdoptedMap> maps = null;
            var resp = new ResourceResponse<List<AdoptedMap>>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();

                maps = await db.AdoptedMaps.AsQueryable().Where(m => m.UserId == userId
                        && m.IsActive == true).ToListAsync<AdoptedMap>();

                timespan.Stop();
                log.TraceApi("SQL Database", String.Format("MyLegacyMapsContext.GetAdoptedMapsByUserIdAsync userId = {0}",
                    userId), timespan.Elapsed);

                resp.Item = maps;
                resp.HttpStatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in MapsRepository.GetAdoptedMapsByUserIdAsync()");
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        public async Task<ResourceResponse<AdoptedMap>> GetAdoptedMapByIdAsync(int id)
        {
            AdoptedMap map = null;
            var resp = new ResourceResponse<AdoptedMap>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();
                map = await db.AdoptedMaps.FindAsync(id);
                if(!map.IsActive)
                {
                    map = null; //map has been deleted
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.GetAdoptedMapByIdAsync", timespan.Elapsed, "id={0}", id);

                resp.Item = map;
                resp.HttpStatusCode = (map != null)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.NotFound;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in AdoptedMapsRepository.GetAdoptedMapByIdAsync(id={0})", id);
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        public async Task<ResourceResponse<AdoptedMap>> CreateAdoptedMapAsync(AdoptedMap adoptedMap)
        {
            var resp = new ResourceResponse<AdoptedMap>();
            try
            {                
                Stopwatch timespan = Stopwatch.StartNew();
                db.AdoptedMaps.Add(adoptedMap);
                var result = await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.AddAdoptedMapAsync", timespan.Elapsed, 
                    "UserId = {0} MapId={1} Name={2}", adoptedMap.UserId, adoptedMap.MapId, adoptedMap.Name);

                bool isSuccess = (result > 0);
                resp.Item = adoptedMap;
                resp.HttpStatusCode = (isSuccess)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in AdoptedMapsRepository.AddAdoptedMapAsync UserId = {0} MapId={1} Name={2}",
                    adoptedMap.UserId, adoptedMap.MapId, adoptedMap.Name));

                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;            
        }

        public async Task<ResourceResponse<AdoptedMap>> SaveAdoptedMapAsync(AdoptedMap adoptedMap)
        {
            var resp = new ResourceResponse<AdoptedMap>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();
                db.Entry(adoptedMap).State = EntityState.Modified;
                var result = await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.SaveAdoptedMapAsync", timespan.Elapsed, 
                    "UserId = {0} MapId={1} Name={2}", adoptedMap.UserId, adoptedMap.MapId, adoptedMap.Name);

                bool isSuccess = (result > 0);
                resp.Item = adoptedMap;
                resp.HttpStatusCode = (isSuccess)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in AdoptedMapsRepository.SaveAdoptedMapAsync UserId = {0} MapId={1} Name={2}",
                    adoptedMap.UserId, adoptedMap.MapId, adoptedMap.Name));

                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        public async Task<ResourceResponse<bool>> DeleteAdoptedMapAsync(AdoptedMap adoptedMap)
        {
            var resp = new ResourceResponse<bool>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();

                adoptedMap.IsActive = false;
                db.Entry(adoptedMap).State = EntityState.Modified;
                var result = await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.DeleteAdoptedMapAsync", timespan.Elapsed,
                    "UserID = {0} AdoptedMapId = {1}", adoptedMap.UserId, adoptedMap.AdoptedMapId);

                bool isSuccess = (result > 0);
                resp.Item = isSuccess;
                resp.HttpStatusCode = (isSuccess)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in AdoptedMapsRepository.DeleteAdoptedMapAsync UserID = {0} AdoptedMapId = {1}",
                    adoptedMap.UserId, adoptedMap.AdoptedMapId));
                    

                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        public async Task<ResourceResponse<List<ShareStatusType>>> GetShareTypesAsync()
        {
            List<ShareStatusType> shareTypes = null;
            var resp = new ResourceResponse<List<ShareStatusType>>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();

                shareTypes = await db.SharedStatusTypes.ToListAsync<ShareStatusType>();

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.GetShareTypesAsync", timespan.Elapsed);

                resp.Item = shareTypes;
                resp.HttpStatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in AdoptedMapsRepository.GetShareTypesAsync()");
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;

        }
        
               

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }
    }
}
