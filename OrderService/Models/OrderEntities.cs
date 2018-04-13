using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderNo { get; set; }
        public string Email { get; set; }
        public Guid PaymentMethodId { get; set; }
        public Guid ShipmentMethodId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public bool NotifyShopper { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderId { get; set; }
        public string CustomerId { get; set; }
        [NotMapped]
        public decimal OrderTotal { get; set; }
    }

    public class OrderItem
    {

        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderItemId { get; set; }
    }
    public class BillingInfo
    {
        public Guid OrderId { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BillingInfoId { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class Product
    {
        [Required]
        public string ProductName { get; set; }
        public string ProductMediaFile { get; set; }
        public string ProductSku { get; set; }
        public Guid ProductCategory { get; set; }
        public string ProductManufacturer { get; set; }
        public decimal Price { get; set; }
        public int ShopperReview { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProductId { get; set; }
    }
}
