// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using PaymentService.Models;
//
//    var linaMpesaOnlineTransactionResponse = LinaMpesaOnlineTransactionResponse.FromJson(jsonString);

namespace PaymentService.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class LinaMpesaOnlineTransactionResponse
    {
        [JsonProperty("Body")]
        public Body Body { get; set; }
    }

    public partial class Body
    {
        [JsonProperty("stkCallback")]
        public StkCallback StkCallback { get; set; }
    }

    public partial class StkCallback
    {
        [JsonProperty("MerchantRequestID")]
        public string MerchantRequestId { get; set; }

        [JsonProperty("CheckoutRequestID")]
        public string CheckoutRequestId { get; set; }

        [JsonProperty("ResultCode")]
        public long ResultCode { get; set; }

        [JsonProperty("ResultDesc")]
        public string ResultDesc { get; set; }

        [JsonProperty("CallbackMetadata")]
        public CallbackMetadata CallbackMetadata { get; set; }
    }

    public partial class CallbackMetadata
    {
        [JsonProperty("Item")]
        public Item[] Item { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
    }

    public partial class LinaMpesaOnlineTransactionResponse
    {
        public static LinaMpesaOnlineTransactionResponse FromJson(string json) => JsonConvert.DeserializeObject<LinaMpesaOnlineTransactionResponse>(json, PaymentService.Models.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this LinaMpesaOnlineTransactionResponse self) => JsonConvert.SerializeObject(self, PaymentService.Models.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = { 
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}