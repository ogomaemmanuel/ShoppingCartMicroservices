using BasketService.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.Services
{
    public class BasketManager : IRepository<BasketItem>
    {
        private readonly IDistributedCache _distributedCache;

        public BasketManager(IDistributedCache distributedCache) {
            _distributedCache = distributedCache;
        }

        public bool Add(BasketItem t, String customerId)
        {
            try
            {
                var customerBasketItems = this.GetByCustomerId(customerId) ?? new List<BasketItem>();
                customerBasketItems.Add(t);
                _distributedCache.SetString(customerId, JsonConvert.SerializeObject(customerBasketItems));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
            
        }

        public bool ClearBasketByCustomerId(string id)
        {
            try
            {
                this._distributedCache.Remove(id);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
          
        }

        public List<BasketItem> GetByCustomerId(string customerId)
        {
         var customerBasketBytes= _distributedCache.GetString(customerId);
            if (customerBasketBytes != null)
            {
                
                var basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(customerBasketBytes);
                
               
                return basketItems;
            }
                return new List<BasketItem>();
        }
    
        public bool RemoveSingleBasketItem(Guid itemId, string customerId)
        {
           var customerBasketItems = this.GetByCustomerId(customerId) ?? new List<BasketItem>();
          var newBasketItems =  customerBasketItems.Where(basketItem => basketItem.ProductId != itemId).Select(basketItem => basketItem).ToList();
          _distributedCache.SetString(customerId, JsonConvert.SerializeObject(newBasketItems));
            return true;
        }

        public bool UpdateBasketItem(BasketItem t, string customerId)
        {
            var customerBasketItems = this.GetByCustomerId(customerId) ?? new List<BasketItem>();
            customerBasketItems.Add(t);
            _distributedCache.SetString(customerId, JsonConvert.SerializeObject(customerBasketItems));
            return true;

        }
    }
}
