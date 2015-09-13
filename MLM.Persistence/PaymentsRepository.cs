using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using MLM.Models;
using MLM.Persistence.Interfaces;
using MLM.Persistence;
using MLM.Logging;

namespace MLM.Persistence
{
    public class PaymentsRepository : IPaymentsRepository, IDisposable
    {
        private MyLegacyMapsContext db = new MyLegacyMapsContext();
        private readonly ILogger log = null;

        public PaymentsRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task<ResourceResponse<List<Payment>>> GetPaymentsAsync(string userId)
        {
            List<Payment> payments = new List<Payment>();
            var resp = new MLM.Persistence.ResourceResponse<List<Payment>>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();

                payments = await db.Payments.AsQueryable()
                    .Where(m => m.UserId == userId).ToListAsync();


                timespan.Stop();
                log.TraceApi("SQL Database", String.Format("PaymentsRepository.GetPaymentsAsync userId = {0}",
                    userId), timespan.Elapsed);

                resp.Item = payments;
                resp.HttpStatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                log.Error(e, String.Format("Error in PaymentsRepository.GetPaymentsAsync userId = {0}",
                    userId));
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        public async Task<ResourceResponse<Payment>> GetPaymentAsync(string transactionId)
        {
            Payment payment = null;
            var resp = new MLM.Persistence.ResourceResponse<Payment>();
            try
            {
                Stopwatch timespan = Stopwatch.StartNew();
                payment = await db.Payments.FindAsync(transactionId);

                timespan.Stop();
                log.TraceApi("SQL Database", "PaymentsRepository.GetPaymentAsync", timespan.Elapsed, "transactionId={0}", transactionId);

                resp.Item = payment;
                resp.HttpStatusCode = (payment != null)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.NotFound;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PaymentsRepository.GetPaymentAsync={0})", transactionId);
                resp.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return resp;
        }

        public async Task<ResourceResponse<Payment>> AddPaymentAsync(Payment payment)
        {
            var resp = new MLM.Persistence.ResourceResponse<Payment>();
            try
            {

                Stopwatch timespan = Stopwatch.StartNew();
                db.Payments.Add(payment);
                var result = await db.SaveChangesAsync();

                timespan.Stop();

                log.TraceApi("SQL Database", "PaymentsRepository.AddPaymentAsync", timespan.Elapsed,
                    "transactionId={0}, userId={1}", payment.TransactionId, payment.UserId);

                bool isSuccess = (result > 0);
                resp.Item = payment;
                resp.HttpStatusCode = (isSuccess)
                    ? System.Net.HttpStatusCode.OK
                    : System.Net.HttpStatusCode.InternalServerError;

            }
            catch (Exception ex)
            {
                log.Error(ex, String.Format("Error in PaymentsRepository.AddPaymentAsync transactionId={0}, userid = {1}",
                    (payment != null)? payment.TransactionId : "null", (payment != null)? payment.UserId : "null"));

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
