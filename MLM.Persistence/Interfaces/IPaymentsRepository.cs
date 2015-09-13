using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLM.Models;
using MLM.Persistence;

namespace MLM.Persistence.Interfaces
{
    public interface IPaymentsRepository
    {
        Task<ResourceResponse<List<Payment>>> GetPaymentsAsync(string userId);
        Task<ResourceResponse<Payment>> GetPaymentAsync(string transactionId);
        Task<ResourceResponse<Payment>> AddPaymentAsync(Payment payment);        
    }
}
