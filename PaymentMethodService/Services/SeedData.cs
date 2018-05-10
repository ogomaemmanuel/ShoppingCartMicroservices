using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentMethodService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ShoppingCartDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ShoppingCartDbContext>>()))
        {

           

            #region PaymentOption
            if (!context.PaymentMethods.Any()){
                context.PaymentMethods.AddRange(
                    new PaymentMethod() {
                    Name="Mpesa",
                    PaymentMethodId= Guid.Parse("816f2387-147e-4bc5-812e-b45cdd862238")
                    },

                     new PaymentMethod()
                     {
                         Name = "Cash on delivery",
                         PaymentMethodId = Guid.Parse("866f8387-147e-4bc5-812e-b45cdd862238")
                     }
                    );
            }
            #endregion PaymentOption
            context.SaveChanges();
        }
    }
}
