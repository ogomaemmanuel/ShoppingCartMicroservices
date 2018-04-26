﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentService.Models;
using PaymentService.Services;
namespace PaymentService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ShoppingCartDbContext>(Options =>
            //{
            //    Options.UseSqlite(Configuration.GetConnectionString("ShoppingCartDbConnectionString"));
            //});

            services.AddSingleton<Func<ShoppingCartDbContext>>(() =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ShoppingCartDbContext>();
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("ShoppingCartDbConnectionString"));
                return new ShoppingCartDbContext(optionsBuilder.Options);
            });
            services.AddSingleton<IOrderPlacedSubscriber, OrderPlacedSubscriber>();
            services.AddTransient<ILipaNaMpesaManager, LipaNaMpesaManager>();
            services.Configure<StkSetting>(options => Configuration.GetSection("StkSetting").Bind(options));
            services.Configure<ShoppingCartStkPushKey>(options => Configuration.GetSection("ShoppingCartStkPushKey").Bind(options));
            services.AddMvc();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRabbitListener();
            app.UseMvc();
            
        }
    }

    public static class ApplicationBuilderExtentions
    {
        public static IOrderPlacedSubscriber Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetRequiredService<IOrderPlacedSubscriber>();

            var life = app.ApplicationServices.GetService<IApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);

            //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
            life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            Listener.Handle();
        }

        private static void OnStopping()
        {
            // Listener.Deregister();
        }
    }
}
