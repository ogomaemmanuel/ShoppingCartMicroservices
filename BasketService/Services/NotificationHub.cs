

using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BasketService.Services
{
    public class NotificationHub : Hub
    {

        public void SendToAll(string name, string message)

        {
            Clients.All.SendAsync("sendToAll", name, message);
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public void RegisterUser(string name) {


        }

    }
}
