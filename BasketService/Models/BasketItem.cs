using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.Models
{
    public class BasketItem
    {
        [JsonProperty("productId")]
        public Guid ProductId { get; set; }
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        [JsonProperty("productMediaFile")]
        public string ProductMediaFile { get; set; }
        [JsonProperty("productSku")]
        public string ProductSku { get; set; }
        [JsonProperty("productCategory")]
        public string ProductCategory { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("shopperReview")]
        public decimal ShopperReview { get; set; }
        public int Quantity { get; set; }
    }
}
