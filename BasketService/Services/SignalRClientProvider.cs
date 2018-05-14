using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace BasketService.Services
{
    public class SignalRClientProvider : ISignalRClientProvider
    {
        public HubConnection HubConnection { get; }
        public SignalRClientProvider()
        {
            HubConnection = new HubConnectionBuilder().WithUrl("https://signalrnotificationservice", HttpTransportType.LongPolling)

                   .ConfigureLogging(logging =>

                    {

                 }).Build();                
        }
        public HubConnection GetHubConnection() {
            return this.GetHubConnection();
        }

        
    }
}
