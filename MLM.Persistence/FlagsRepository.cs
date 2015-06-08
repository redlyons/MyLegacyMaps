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
    public class FlagsRepository : IFlagsRepository, IDisposable
    {
        private MyLegacyMapsContext db = new MyLegacyMapsContext();
        private readonly ILogger log = null;

        public FlagsRepository(ILogger logger)
        {
            log = logger;
        }        

        public async Task<ResourceResponse<Flag>> FindFlagByIdAsync(int id)
        {
            Flag flag = null;
            var resp = new ResourceResponse<Flag>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();
                flag = await db.Flags.FindAsync(id);

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.FindFlagByIdAsync", timespan.Elapsed, "id={0}", id);

                resp.Item = flag;
                resp.HttpStatusCode = (flag != null)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.NotFound;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in AdoptedMapsRepository.FindFlagByIdAsync(id={0})", id);
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        public async Task<ResourceResponse<Flag>> AddFlagAsync(Flag flag)
        {
            var resp = new ResourceResponse<Flag>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();
                db.Flags.Add(flag);
                var result = await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.AddFlagAsync", timespan.Elapsed,
                    "FlagId = {0}", flag.FlagId);

                bool isSuccess = (result > 0);
                resp.Item = flag;
                resp.HttpStatusCode = (isSuccess)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in FlagsRepository.AddFlagAsync FlagId = {0}",
                    flag.FlagId));

                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;       
        }

        public async Task<ResourceResponse<Flag>> SaveFlagAsync(Flag flag)
        {
            var resp = new ResourceResponse<Flag>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();
                db.Entry(flag).State = EntityState.Modified;
                var result = await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.SaveFlagAsync", timespan.Elapsed,
                    "FlagId = {0}", flag.FlagId);

                bool isSuccess = (result > 0);
                resp.Item = flag;
                resp.HttpStatusCode = (isSuccess)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in FlagsRepository.SaveFlagAsync = {0}",
                    flag.FlagId));

                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        public async Task<ResourceResponse<bool>> DeleteFlagAsync(Flag flag)
        {
            var resp = new ResourceResponse<bool>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();

                db.Flags.Remove(flag);
                var result = await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "MyLegacyMapsContext.DeleteFlagAsync", timespan.Elapsed,
                    "FlagId = {0}", flag.FlagId);

                bool isSuccess = (result > 0);
                resp.Item = isSuccess;
                resp.HttpStatusCode = (isSuccess)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in FlagsRepository.DeleteFlagAsync FlagId = {0}",
                    flag.FlagId));


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
