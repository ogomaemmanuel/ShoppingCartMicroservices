using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductService.Conventions;
using ProductService.Models;
using ProductService.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace ProductService
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
            services.AddDbContext<ShoppingCartDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ShoppingCartDbConnectionString"),
            sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
            })
            );

            services.AddTransient<IRepository<Product>, ProductsManager>();


            services.AddMvc(options =>
            {
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
            services.AddSwaggerGen(c =>

            {

                c.SwaggerDoc("v1", new Info { Title = "Shopping Cart Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                                                 {
                                       { "Bearer", new string[] { } }
                                    });

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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping Cart V1");

            });
            app.UseAuthentication();
            app.UseMvc();
        }

    }
}
