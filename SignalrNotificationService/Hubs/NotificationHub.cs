using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalrNotificationService.SystemIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalrNotificationService.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {
        IUserLoggedInRedisPublisher _userLoggedInRedisPublisher;
        public NotificationHub(IUserLoggedInRedisPublisher userLoggedInRedisPublisher)
        {
            _userLoggedInRedisPublisher = userLoggedInRedisPublisher;
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public void SendToAll(string user, string message)
        {
            Clients.All.SendToAll(user, message);
        }
        public async Task RegisterUser(string userId)
        {
            UserLoggedInMessage userLoggedInMessage = new UserLoggedInMessage()
            {
                UserId = userId,
            };
           //TODO: ToString be tested later
           // _userLoggedInRedisPublisher.Publish(userLoggedInMessage);
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            
        }
        public void BasketChangedMessage(string customerId, string message) {
            Clients.Group(customerId).SendToAll("BasketChanged", message);
        }

    }
}
