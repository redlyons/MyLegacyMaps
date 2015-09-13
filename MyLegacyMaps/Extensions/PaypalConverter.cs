using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MLM.Models;
using MyLegacyMaps.Classes.Paypal;

namespace MyLegacyMaps.Extensions
{
    public static class PayPalConverter
    {

        public static MLM.Models.Payment ToPaymentModel(this PDTHolder value, string userId)
        {
            if(value == null)
                return null;

            return new Payment()
            {
                TransactionId = value.TransactionId,
                UserId = userId,
                GrossTotal = value.GrossTotal,
                Currency = value.Currency,
                PayerFirstName = value.PayerFirstName,
                PayerLastName = value.PayerLastName,
                PayerEmail = value.PayerEmail,
                Tokens = value.Tokens,
                TransactionDate = value.TransactionDate,
                TransactionDetails = value.TransactionDetails,
                TransactionStatus = value.TransactionStatus

            };
        }
    }


}