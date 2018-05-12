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
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        public BasketManager(IDistributedCache distributedCache, IHubContext<NotificationHub> notificationHubContext) {
            _distributedCache = distributedCache;
            _notificationHubContext = notificationHubContext;
        }

        public bool Add(BasketItem t, String customerId)
        {
            try
            {
               var x= _notificationHubContext.Clients.All;
                var customerBasketItems = this.GetByCustomerId(customerId) ?? new List<BasketItem>();
                customerBasketItems.Add(t);
                _distributedCache.SetString(customerId, JsonConvert.SerializeObject(customerBasketItems));
                _notificationHubContext.Clients.All.SendAsync("sendToAll", "Emmanuel", customerBasketItems.Count().ToString());
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
                _notificationHubContext.Clients.All.SendAsync("sendToAll", "Emmanuel", "0");
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
            _notificationHubContext.Clients.All.SendAsync("sendToAll", "Emmanuel", newBasketItems.Count().ToString());
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
