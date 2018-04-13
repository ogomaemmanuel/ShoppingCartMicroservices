using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
    /// <summary>
    /// contains classes used when querying the service
    /// </summary>
    public class ViewModels
    {
    }

    public class CustomerOrder
    {
        public string Email { get; set; }
        public Guid PaymentMethodId { get; set; }
        public Guid ShipmentMethodId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string Status { get; set; }
        public bool NotifyShopper { get; set; }
        public Guid? OrderId { get; set; }
        public string CustomerId { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public BillingInfoViewModel BillingInfo { get; set; }

    }
    public class OrderItemViewModel
    {
        public Guid ProductId { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
    public class BillingInfoViewModel
    {
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
    }
}
