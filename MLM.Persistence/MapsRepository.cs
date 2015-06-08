﻿using System;
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
                    maps = await db.Maps.AsQueryable().Where(m => m.MapTypeId == mapTypeId).ToListAsync();
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

        public async Task<ResourceResponse<Map>> FindMapByIdAsync(int id)
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
                resp.HttpStatusCode = (map !=null)
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
