using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Models
{
    public class LipaNaMpesaPayment
    {
        
        public Guid LipaNaMpesaPaymentId { get; set; }
        public Guid OrderId { get; set; }
        [Key]
        public string MerchantRequestId { get; set; }
        public string CheckoutRequestId { get; set; }
        public string ResultDesc { get; set; }      
        public decimal? Amount { get; set; }
        public string MpesaReceiptNumber { get; set; }
        public string TransactionDate { get; set; }
        public string PhoneNumber { get; set; }
    }
}
