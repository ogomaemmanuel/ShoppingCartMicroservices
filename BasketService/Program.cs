﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BasketService.Services;
using BasketService.SystemIntegration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BasketService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                   
                    var orderPlacedSubsriber = services.GetRequiredService<IBasketChangedHandler>();
                    orderPlacedSubsriber.Handle();

                    //TODO: tob worked on later, not working as expected
                    //var UserLoggedInSubscriber = services.GetRequiredService<UserLoggedInSubscriber>();
                    //UserLoggedInSubscriber.Subscribe();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()                
                .Build();


        
    }

  
}
