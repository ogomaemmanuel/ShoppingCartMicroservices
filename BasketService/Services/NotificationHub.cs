

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
    public class NotificationHub : Hub<INotificationHubClient>
    {
        
        private readonly IDistributedCache _distributedCache;
        public NotificationHub(IDistributedCache distributedCache) {
            _distributedCache = distributedCache;
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public void SendToAll(string user,string message ) {
            Clients.All.SendToAll(user, message);
        }

        public async Task RegisterUser(string userId) {
           await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            var customerBasketBytes = await _distributedCache.GetStringAsync(userId);
            var customerBasketItems=JsonConvert.DeserializeObject<List<BasketItem>>(customerBasketBytes) ?? new List<BasketItem>();
            Clients.Group(userId).SendToAll("BasketChanged", customerBasketItems.Count().ToString());
        }

       

    }
}
