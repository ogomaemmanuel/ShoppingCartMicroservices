using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.Services
{
   public interface INotificationHubClient
    {
        Task SendToAll(string name, string message);
        Task SendToOne(string messageType,object message);
    }
}
