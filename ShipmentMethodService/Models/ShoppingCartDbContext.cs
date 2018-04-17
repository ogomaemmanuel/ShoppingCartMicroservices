using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShipmentMethodService.Models
{
    public class ShoppingCartDbContext:DbContext
    {

        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options)
         : base(options)
        {
            Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
      
        public DbSet<ShipmentMethod> ShipmentMethods { get; set; }
        
    }
}
