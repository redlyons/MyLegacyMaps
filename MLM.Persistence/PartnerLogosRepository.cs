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
    public class PartnerLogosRepository : IPartnerLogosRepository, IDisposable
    {
        private MyLegacyMapsContext db = new MyLegacyMapsContext();
        private readonly ILogger log = null;

        public PartnerLogosRepository(ILogger logger)
        {
            log = logger;
        }


        public async Task<ResourceResponse<List<PartnerLogo>>> GetPartnerLogosAsync()
        {
            List<PartnerLogo> logos = null;
            var resp = new ResourceResponse<List<PartnerLogo>>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();

                logos = await db.PartnerLogos.AsQueryable().Where(pl => pl.IsActive).ToListAsync<PartnerLogo>();

                timespan.Stop();
                log.TraceApi("SQL Database", "PartnerLogosRepository.GetPartnerLogosAsync", timespan.Elapsed);

                resp.Item = logos;
                resp.HttpStatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PartnerLogosRepository.GetPartnerLogosAsync()");
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;

        }


        public async Task<ResourceResponse<List<PartnerLogo>>> AdminGetPartnerLogosAsync()
        {
            List<PartnerLogo> logos = null;
            var resp = new ResourceResponse<List<PartnerLogo>>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();

                logos = await db.PartnerLogos.ToListAsync<PartnerLogo>();

                timespan.Stop();
                log.TraceApi("SQL Database", "PartnerLogosRepository.AdminGetPartnerLogosAsync", timespan.Elapsed);

                resp.Item = logos;
                resp.HttpStatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PartnerLogosRepository.AdminGetPartnerLogosAsync()");
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;

        }

        public async Task<ResourceResponse<PartnerLogo>> AdminGetPartnerLogoAsync(int id)
        {
            PartnerLogo type = null;
            var resp = new ResourceResponse<PartnerLogo>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();
                type = await db.PartnerLogos.FindAsync(id);

                timespan.Stop();
                log.TraceApi("SQL Database", "PartnerLogosRepository.AdminGetParnterLogoAsync", timespan.Elapsed, "id={0}", id);

                resp.Item = type;
                resp.HttpStatusCode = (type != null)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.NotFound;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PartnerLogosRepository.AdminGetParnterLogoAsync(id={0})", id);
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        public async Task<ResourceResponse<PartnerLogo>> AdminSavePartnerLogoAsync(PartnerLogo logo)
        {
            var resp = new ResourceResponse<PartnerLogo>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();
                db.Entry(logo).State = EntityState.Modified;
                var result = await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "PartnerLogosRepository.AdminSaveParnterLogoAsync", timespan.Elapsed,
                    "PartnerLogoId = {0} Name={1}", logo.PartnerLogoId, logo.Name);

                bool isSuccess = (result > 0);
                resp.Item = logo;
                resp.HttpStatusCode = (isSuccess)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in PartnerLogosRepository.AdminSaveParnterLogoAsync PartnerLogoId = {0} Name={1}",
                   logo.PartnerLogoId, logo.Name));

                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        public async Task<ResourceResponse<PartnerLogo>> AdminCreatePartnerLogoAsync(PartnerLogo logo)
        {
            var resp = new ResourceResponse<PartnerLogo>();
            try
            {

                Stopwatch timespan = Stopwatch.StartNew();
                db.PartnerLogos.Add(logo);
                var result = await db.SaveChangesAsync();

                timespan.Stop();

                log.TraceApi("SQL Database", "PartnerLogosRepository.AdminCreateParnterLogoAsync", timespan.Elapsed,
                    "Name={0}", logo.Name);

                bool isSuccess = (result > 0);
                resp.Item = logo;
                resp.HttpStatusCode = (isSuccess)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.InternalServerError;

            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in PartnerLogosRepository.AdminCreateParnterLogoAsync Name={0}",
                   logo.Name));

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
