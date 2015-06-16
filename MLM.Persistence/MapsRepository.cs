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
    public class MapsRepository : IMapsRepository, IDisposable
    {
        private MyLegacyMapsContext db = new MyLegacyMapsContext();
        private readonly ILogger log = null;

        public MapsRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task<ResourceResponse<List<Map>>> GetMapsAsync(int mapTypeId = 0)
        {
            List<Map> maps = null;           
            var resp = new ResourceResponse<List<Map>>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();
               
              
                if (mapTypeId > 0)
                {
                    maps = await db.Maps.AsQueryable().Where(m => m.IsActive == true
                         && m.MapTypes.Any(i => i.MapTypeId == mapTypeId)).ToListAsync();
                }
                else
                {
                    maps = await db.Maps.AsQueryable().Where(m => m.IsActive == true).ToListAsync();
                }
                             

                timespan.Stop();
                log.TraceApi("SQL Database", String.Format("MyLegacyMapsContext.GetMapsAsync mapTypeId = {0}", 
                    mapTypeId), timespan.Elapsed);

                resp.Item = maps;
                resp.HttpStatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in MapsRepository.GetMapsAsync()");
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;

        }

       

        public async Task<ResourceResponse<List<MapType>>> GetMapTypesAsync()
        {
            List<MapType> mapTypes = null;
            var resp = new ResourceResponse<List<MapType>>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();

                mapTypes = await db.MapTypes.AsQueryable().Where(mt => mt.IsActive == true).ToListAsync<MapType>();

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.GetMapTypesAsync", timespan.Elapsed);

                resp.Item = mapTypes;
                resp.HttpStatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in MapsRepository.GetMapTypesAsync()");
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;

        }

        public async Task<ResourceResponse<Map>> GetMapAsync(int id)
        {
            Map map = null;           
            var resp = new ResourceResponse<Map>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();
                map = await db.Maps.FindAsync(id);

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.FindTaskByIdAsync", timespan.Elapsed, "id={0}", id);

                resp.Item = map;
                resp.HttpStatusCode = (map !=null && map.IsActive)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.NotFound;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in MapsRepository.FindTaskByIdAsync(id={0})", id);
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }


        public async Task<ResourceResponse<Map>> AdminCreateMapAsync(Map map)
        {
            var resp = new ResourceResponse<Map>();
            try
            {
              
                    Stopwatch timespan = Stopwatch.StartNew();
                    db.Maps.Add(map);
                    var result = await db.SaveChangesAsync();

                    timespan.Stop();

                    log.TraceApi("SQL Database", "MyLegacyMapsContext.CreateMapAsync", timespan.Elapsed,
                        "Name={0}", map.Name);

                    bool isSuccess = (result > 0);
                    resp.Item = map;
                    resp.HttpStatusCode = (isSuccess)
                        ? System.Net.HttpStatusCode.OK
                        : System.Net.HttpStatusCode.InternalServerError;
               
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in MapsRepository.CreateMapAsync Name={0}",
                   map.Name));

                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;       
        }

        public async Task<ResourceResponse<Map>> AdminSaveMapAsync(Map map)
        {
            var resp = new ResourceResponse<Map>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();               
                db.Entry(map).State = EntityState.Modified;
                var result = await db.SaveChangesAsync();    


                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.SaveAdoptedMapAsync", timespan.Elapsed,
                    "MapId = {0} Name={1}", map.MapId, map.Name);

                bool isSuccess = (result > 0);
                resp.Item = map;
                resp.HttpStatusCode = (isSuccess)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in MapsRepository.SaveMapAsync MapId = {0} Name={1}",
                    map.MapId, map.Name));

                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        public async Task<ResourceResponse<Map>> AdminSaveMapTypesAsync(int mapId, List<int> mapTypeIds)
        {
            var resp = new ResourceResponse<Map>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();

                var map = db.Maps.Include(m => m.MapTypes).Single(s => s.MapId == mapId);
                var mapTypesResp = await this.AdminGetMapTypesAsync();
                if (map == null)
                {
                    resp.HttpStatusCode = System.Net.HttpStatusCode.NotFound;
                    return resp;
                }
                if (!mapTypesResp.IsSuccess())
                {
                    resp.HttpStatusCode = mapTypesResp.HttpStatusCode;
                    return resp;
                }
               
                var allMapTypes = mapTypesResp.Item;
                var updatedMapTypes = new List<MapType>();
                foreach (var id in mapTypeIds)
                {
                    MapType type = allMapTypes.Find(mt => mt.MapTypeId == id);
                    if (type != null)
                    {
                        updatedMapTypes.Add(type);
                    }
                }

                if (map.MapTypes.Equals(updatedMapTypes))
                {
                    resp.HttpStatusCode = System.Net.HttpStatusCode.OK;
                    return resp;
                }
                List<MapType> clonedMapTypes = new List<MapType>();
                foreach (var curr in map.MapTypes)
                {
                    clonedMapTypes.Add(curr);
                }
                //Delete all map types than re-add
                foreach (var curr in clonedMapTypes)
                {
                    MapType typeToDel = allMapTypes.Find(mt => mt.MapTypeId == curr.MapTypeId);
                    map.MapTypes.Remove(typeToDel);
                }

                var resultDel = await db.SaveChangesAsync();
                bool isSuccess = (resultDel >= 0);
                if (!isSuccess)
                {
                    resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                    return resp;
                }

                //Add upated map types
                foreach (var type in updatedMapTypes)
                {
                    MapType typeToAdd = allMapTypes.Find(mt => mt.MapTypeId == type.MapTypeId);
                    map.MapTypes.Add(typeToAdd);
                }
                var resultAdd = await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL =Database", "MyLegacyMapsContext.AdminSaveMapTypesAsync", timespan.Elapsed,
                    "MapId = {0}", mapId);

                isSuccess = (resultAdd >= 0);
                resp.Item = map;
                resp.HttpStatusCode = (isSuccess)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in MapsRepository.AdminSaveMapTypesAsync MapId = {0}",
                    mapId));

                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        /// <summary>
        /// Admin Only function
        /// Gets a list of maps to manage (ignores isActive flag).
        /// </summary>
        /// <param name="mapTypeId"></param>
        public async Task<ResourceResponse<List<Map>>> AdminGetMapsAsync(int mapTypeId = 0)
        {
            List<Map> maps = null;
            var resp = new ResourceResponse<List<Map>>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();
                if (mapTypeId > 0)
                {
                    maps = await db.Maps.AsQueryable().Where(m =>
                        m.MapTypes.Any(i => i.MapTypeId == mapTypeId)).ToListAsync();
                }
                else
                {
                    maps = await db.Maps.ToListAsync();
                }

                timespan.Stop();
                log.TraceApi("SQL Database", String.Format("MyLegacyMapsContext.GetMapsAsync mapTypeId = {0}",
                    mapTypeId), timespan.Elapsed);

                resp.Item = maps;
                resp.HttpStatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in MapsRepository.GetMapsAsync()");
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public async Task<ResourceResponse<Map>> AdminGetMapAsync(int id)
        {
            Map map = null;
            var resp = new ResourceResponse<Map>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();
                map = await db.Maps.FindAsync(id);

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.AdminGetMapAsync", timespan.Elapsed, "id={0}", id);

                resp.Item = map;
                resp.HttpStatusCode = (map != null)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.NotFound;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in MapsRepository.AdminGetMapAsync(id={0})", id);
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        public async Task<ResourceResponse<List<MapType>>> AdminGetMapTypesAsync()
        {
            List<MapType> mapTypes = null;
            var resp = new ResourceResponse<List<MapType>>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();

                mapTypes = await db.MapTypes.ToListAsync<MapType>();

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.AdminGetMapTypesAsync", timespan.Elapsed);

                resp.Item = mapTypes;
                resp.HttpStatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in MapsRepository.AdminGetMapTypesAsync()");
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;

        }

        public async Task<ResourceResponse<MapType>> AdminGetMapTypeAsync(int id)
        {
            MapType type = null;
            var resp = new ResourceResponse<MapType>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();
                type = await db.MapTypes.FindAsync(id);

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.AdminGetMapTypeAsync", timespan.Elapsed, "id={0}", id);

                resp.Item = type;
                resp.HttpStatusCode = (type != null)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.NotFound;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in MapsRepository.AdminGetMapTypeAsync(id={0})", id);
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        public async Task<ResourceResponse<MapType>> AdminSaveMapTypeAsync(MapType mapType)
        {
            var resp = new ResourceResponse<MapType>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();
                db.Entry(mapType).State = EntityState.Modified;
                var result = await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.AdminSaveMapTypeAsync", timespan.Elapsed,
                    "MapId = {0} Name={1}", mapType.MapTypeId, mapType.Name);

                bool isSuccess = (result > 0);
                resp.Item = mapType;
                resp.HttpStatusCode = (isSuccess)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in MapsRepository.AdminSaveMapTypeAsync MapId = {0} Name={1}",
                    mapType.MapTypeId, mapType.Name));

                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        public async Task<ResourceResponse<MapType>> AdminCreateMapTypeAsync(MapType mapType)
        {
            var resp = new ResourceResponse<MapType>();
            try
            {

                Stopwatch timespan = Stopwatch.StartNew();
                db.MapTypes.Add(mapType);
                var result = await db.SaveChangesAsync();

                timespan.Stop();

                log.TraceApi("SQL Database", "MyLegacyMapsContext.AdminCreateMapTypeAsync", timespan.Elapsed,
                    "Name={0}", mapType.Name);

                bool isSuccess = (result > 0);
                resp.Item = mapType;
                resp.HttpStatusCode = (isSuccess)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.InternalServerError;

            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in MapsRepository.AdminCreateMapTypeAsync Name={0}",
                   mapType.Name));

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
