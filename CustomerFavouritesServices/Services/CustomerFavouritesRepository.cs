using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerFavouritesServices.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CustomerFavouritesServices.Services
{
    public class CustomerFavouritesManager : IRepository<Product>
    {
        private readonly IDistributedCache _distributedCache;
        public CustomerFavouritesManager(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public bool Add(Product newProduct, string customerId)
        {
            var customerFavourites = this.GetByCustomerId(customerId) ?? new List<Product>();
            customerFavourites.Add(newProduct);
            _distributedCache.SetString(customerId, JsonConvert.SerializeObject(customerFavourites));
            return true;
        }

        public bool ClearCustomerFovouritesByCustomerId(string customerId)
        {
            try
            {
                this._distributedCache.Remove(customerId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int Count(string CustomerId)
        {
            var customerFavouritesProducts = this.GetByCustomerId(CustomerId);
            return customerFavouritesProducts.Count();
        }

        public List<Product> GetByCustomerId(string customerId)
        {
            var customerFovouritesJsonString = _distributedCache.GetString(customerId);
            if (customerFovouritesJsonString != null)
            {
                var customerFovourites = JsonConvert.DeserializeObject<List<Product>>(customerFovouritesJsonString);
                return customerFovourites;
            }
            return new List<Product>();
        }
        public bool RemoveSingleFovouriteItem(Guid itemId, string customerId)
        {
            var customerFavouriteItems = this.GetByCustomerId(customerId) ?? new List<Product>();
            var newcustomerFavouriteItems = customerFavouriteItems.Where(favourite => favourite.ProductId != itemId).Select(favourite => favourite).ToList();
            _distributedCache.SetString(customerId, JsonConvert.SerializeObject(newcustomerFavouriteItems));
            return true;
        }
    }
}
