using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShipmentMethodService.Models;
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

           

            #region ShipmentMethod
            if (!context.ShipmentMethods.Any()) {
                context.ShipmentMethods.AddRange(
                    new ShipmentMethod() {
                    Name="Self pick up",
                    ShipmentMethodId = Guid.Parse("866f8387-147e-4bc5-812e-b45cdd862238")
                    },
                     new ShipmentMethod()
                     {
                         Name = "Default shipment method",
                         ShipmentMethodId = Guid.Parse("866f3387-147e-4bc5-812e-b45c8d862238")
                     }
                    );
            }
            #endregion ShipmentMethod               
           
            context.SaveChanges();
        }
    }
}
