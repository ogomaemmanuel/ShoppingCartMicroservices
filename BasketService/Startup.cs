using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasketService.Conventions;
using BasketService.Models;
using BasketService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BasketService
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
            services.AddTransient<IRepository<BasketItem>, BasketManager>();
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration["Redis:Url"];
            });
            services.AddSingleton<IOrderPlacedSubsriber,OrderPlacedSubscriber>();
            services.AddMvc(options => {
                options.Conventions.Add(new ComplexTypeConvention());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
            {

                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().Build();

            });
            app.UseRabbitListener();
            app.UseMvc();
        }
    }

    public static class ApplicationBuilderExtentions
    {
        public static IOrderPlacedSubsriber Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<IOrderPlacedSubsriber>();

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
