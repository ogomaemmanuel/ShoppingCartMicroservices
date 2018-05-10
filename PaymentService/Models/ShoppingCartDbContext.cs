using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Models
{
    public class ShoppingCartDbContext : DbContext
    {
        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) :
            base(options)
        {

            Database.EnsureCreated();
        }
        public DbSet<LipaNaMpesaPayment> LipaNaMpesaPayments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           

            base.OnConfiguring(optionsBuilder);
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<LipaNaMpesaPayment>()
        //   .HasKey(c => new { c.CheckoutRequestId, c.MerchantRequestId });
        //}
    }
}
