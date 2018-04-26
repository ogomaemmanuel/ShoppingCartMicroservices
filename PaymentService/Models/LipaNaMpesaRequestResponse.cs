using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Models
{
    public class LipaNaMpesaRequestResponse
    {
        [JsonProperty("MerchantRequestID")]
        public string MerchantRequestId { get; set; }

        [JsonProperty("CheckoutRequestID")]
        public string CheckoutRequestId { get; set; }

        [JsonProperty("ResponseCode")]
        public string ResponseCode { get; set; }

        [JsonProperty("ResponseDescription")]
        public string ResponseDescription { get; set; }

        [JsonProperty("CustomerMessage")]
        public string CustomerMessage { get; set; }       
    }
}
