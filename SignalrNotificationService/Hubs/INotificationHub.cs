using SignalrNotificationService.SystemIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalrNotificationService.Hubs
{
    public interface INotificationHub
    {
        Task SendToAll(string messageType, object message);
        
    }
}
