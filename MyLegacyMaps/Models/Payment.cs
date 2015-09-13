using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLegacyMaps.Models
{
    public class Payment
    {
        public string TransactionId { get; set; }
        public string UserId { get; set; }
        public decimal GrossTotal { get; set; }
        public string Currency { get; set; }
        public string PayerFirstName { get; set; }
        public string PayerLastName { get; set; }
        public string PayerEmail { get; set; }
        public int Tokens { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionStatus { get; set; }       
    }
}