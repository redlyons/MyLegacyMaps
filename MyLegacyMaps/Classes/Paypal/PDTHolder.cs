using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLegacyMaps.Classes.Paypal
{
    public class PDTHolder
    {
        public decimal GrossTotal { get; set; }
        public int InvoiceNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string PayerFirstName { get; set; }
        public double PaymentFee { get; set; }
        public string BusinessEmail { get; set; }
        public string PayerEmail { get; set; }
        public string TxToken { get; set; }
        public string PayerLastName { get; set; }
        public string ReceiverEmail { get; set; }
        public string ItemName { get; set; }
        public string Currency { get; set; }
        public string TransactionId { get; set; }
        public string SubscriberId { get; set; }
        public string Custom { get; set; }
        public string Option { get; set; }
        public int Tokens { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionDetails { get; set; }
        public DateTime TransactionDate { get; set; }

        //SUCCESS
        //mc_gross=14.85
        //protection_eligibility=Ineligible
        //payer_id=KVZEYY58CUXNA
        //tax=0.00
        //payment_date=20%3A33%3A51+Sep+12%2C+2015+PDT
        //payment_status=Completed
        //charset=windows-1252
        //first_name=Pink
        //option_selection1=20+Tokens+%28Best+Value%29
        //mc_fee=0.73
        //custom=
        //payer_status=verified
        //business=mylegacymaps%2Bfacilitator1%40gmail.com
        //quantity=1
        //payer_email=mylegacymaps%2BPinkFloyd%40gmail.com
        //option_name1=Quantity
        //txn_id=34046959F2034301W
        //payment_type=instant
        //btn_id=3211161
        //last_name=Floyd
        //receiver_email=mylegacymaps%2Bfacilitator1%40gmail.com
        //payment_fee=0.73
        //shipping_discount=0.00
        //insurance_amount=0.00
        //receiver_id=5P2666BJ77Y2S
        //txn_type=web_accept
        //item_name=MLM+Token
        //discount=0.00
        //mc_currency=USD
        //item_number=
        //residence_country=US
        //handling_amount=0.00
        //shipping_method=Default
        //transaction_subject=
        //payment_gross=14.85
        //shipping=0.00

        public static PDTHolder Parse(string postData)
        {
            String sKey, sValue;
            PDTHolder ph = new PDTHolder();

            try
            {
                ph.TransactionDate = System.DateTime.Now;
                ph.TransactionDetails = postData;
                ph.TransactionStatus = (postData.ToUpper().StartsWith("SUCCESS"))
                    ? "SUCCESS" : "ERROR";


                //split response into string array using whitespace delimeter
                String[] StringArray = postData.Split('\n');

                // NOTE:
                /*
                * loop is set to start at 1 rather than 0 because first
                string in array will be single word SUCCESS or FAIL
                Only used to verify post data
                */

                // use split to split array we already have using "=" as delimiter
                int i;
                for (i = 1; i < StringArray.Length - 1; i++)
                {
                    String[] StringArray1 = StringArray[i].Split('=');

                    sKey = StringArray1[0];
                    sValue = HttpUtility.UrlDecode(StringArray1[1]);

                    // set string vars to hold variable names using a switch
                    switch (sKey)
                    {
                        case "mc_gross":
                            ph.GrossTotal = Convert.ToDecimal(sValue);
                            break;

                        case "invoice":
                            ph.InvoiceNumber = Convert.ToInt32(sValue);
                            break;

                        case "payment_status":
                            ph.PaymentStatus = Convert.ToString(sValue);
                            break;

                        case "first_name":
                            ph.PayerFirstName = Convert.ToString(sValue);
                            break;

                        case "mc_fee":
                            ph.PaymentFee = Convert.ToDouble(sValue);
                            break;

                        case "business":
                            ph.BusinessEmail = Convert.ToString(sValue);
                            break;

                        case "payer_email":
                            ph.PayerEmail = Convert.ToString(sValue);
                            break;

                        case "Tx Token":
                            ph.TxToken = Convert.ToString(sValue);
                            break;

                        case "last_name":
                            ph.PayerLastName = Convert.ToString(sValue);
                            break;

                        case "receiver_email":
                            ph.ReceiverEmail = Convert.ToString(sValue);
                            break;

                        case "item_name":
                            ph.ItemName = Convert.ToString(sValue);
                            break;

                        case "mc_currency":
                            ph.Currency = Convert.ToString(sValue);
                            break;

                        case "txn_id":
                            ph.TransactionId = Convert.ToString(sValue);
                            break;

                        case "custom":
                            ph.Custom = Convert.ToString(sValue);
                            break;

                        case "subscr_id":
                            ph.SubscriberId = Convert.ToString(sValue);
                            break;
                        
                        case "option_selection1":
                            ph.Option = Convert.ToString(sValue);

                            var amt = ph.Option.Substring(0, ph.Option.IndexOf(" "));
                            int tokens = 0;
                            if (Int32.TryParse(amt, out tokens))
                            {
                                ph.Tokens = tokens;
                            }
                            break;

                       
                    }
                }
                

                return ph;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}