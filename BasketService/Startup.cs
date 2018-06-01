using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasketService.Conventions;
using BasketService.Models;
using BasketService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using BasketService.SystemIntegration;

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
            services.AddSingleton<IBasketChangedHandler, BasketChangedHandler>();
            services.AddTransient<IBasketChangedNotificationSender, BasketChangedNotificationSender>();
            services.AddSingleton<ISignalRClientProvider, SignalRClientProvider>();
            services.AddSingleton<UserLoggedInSubscriber>();
            services.AddTransient< IRepository <BasketItem>, BasketManager > ();
            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .WithOrigins("http://localhost:8100")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    );
            }); 
            services.AddMvc(options => {
                options.Conventions.Add(new ComplexTypeConvention());
            });
            
            services
     .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
     {
         options.Authority = Configuration["Jwt:Issuer"];
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidIssuer = Configuration["Jwt:Issuer"],
             ValidateAudience = true,
             ValidAudience = Configuration["Jwt:aud"],
             ValidateLifetime = true
         };
     });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
           
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseMvc();
        }
    }
    }
