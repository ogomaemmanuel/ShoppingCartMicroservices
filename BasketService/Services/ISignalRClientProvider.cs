using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.Services
{
    public interface ISignalRClientProvider
    {
        HubConnection GetHubConnection();
    }
}
