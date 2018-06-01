using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerFavouritesServices.Models
{
    public class Product
    {
       
        public string ProductName { get; set; }
        public string ProductMediaFile { get; set; }
        public string ProductSku { get; set; }
        public Guid ProductCategory { get; set; }
        public string ProductManufacturer { get; set; }
        public decimal Price { get; set; }
        public int ShopperReview { get; set; }
        public Guid ProductId { get; set; }
        public string ProductBaseUrl { get; set; }
    }
}
