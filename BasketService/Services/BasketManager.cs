using BasketService.Models;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IBasketChangedNotificationSender _basketChangedNotificationSender;
        public BasketManager(IDistributedCache distributedCache, IBasketChangedNotificationSender basketChangedNotificationSender) {
            _distributedCache = distributedCache;
            _basketChangedNotificationSender = basketChangedNotificationSender;
        }

        public bool Add(BasketItem t, String customerId)
        {
            try
            {
              
                var customerBasketItems = this.GetByCustomerId(customerId) ?? new List<BasketItem>();
                customerBasketItems.Add(t);
                _distributedCache.SetString(customerId, JsonConvert.SerializeObject(customerBasketItems));
                _basketChangedNotificationSender.PublishCustomerBasketTotal(customerId, customerBasketItems.Count().ToString());
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

        public int Count(string customerId)
        {
            var basketItemsCount = this.GetByCustomerId(customerId).Count();
            return basketItemsCount;
        }

        public bool RemoveSingleBasketItem(Guid itemId, string customerId)
        {
          var customerBasketItems = this.GetByCustomerId(customerId) ?? new List<BasketItem>();
          var newBasketItems =  customerBasketItems.Where(basketItem => basketItem.ProductId != itemId).Select(basketItem => basketItem).ToList();
          _distributedCache.SetString(customerId, JsonConvert.SerializeObject(newBasketItems));
          _basketChangedNotificationSender.PublishCustomerBasketTotal(customerId, newBasketItems.Count().ToString());
            return true;
        }

        public bool UpdateBasketItem(BasketItem t, string customerId)
        {
            var customerBasketItems = this.GetByCustomerId(customerId) ?? new List<BasketItem>();
            customerBasketItems.Add(t);
            _distributedCache.SetString(customerId, JsonConvert.SerializeObject(customerBasketItems));
            _basketChangedNotificationSender.PublishCustomerBasketTotal(customerId, customerBasketItems.Count().ToString());
            return true;
        }
       
    }
}
