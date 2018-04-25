using PaymentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Services
{
    public class LipaNaMpesaManager : ILipaNaMpesaManager
    {
        private readonly ShoppingCartDbContext shoppingCartDbContext;
        public LipaNaMpesaManager(ShoppingCartDbContext shoppingCartDbContext)
        {
            this.shoppingCartDbContext = shoppingCartDbContext;
        }

        public void MakeMpesaPaymentRequest(CustomerOrder customerOrder)
        {
            
        }

        private void GetMpesaAuthenticationToken() { }

        public LipaNaMpesaResponse Save(LinaMpesaOnlineTransactionResponse
            linaMpesaOnlineTransactionResponse)
        {
            try
            {
             var lipaNaMpesaPayment=   shoppingCartDbContext.
                    LipaNaMpesaPayments
                    .Find(new[]{linaMpesaOnlineTransactionResponse.Body.StkCallback.CheckoutRequestId,
                     linaMpesaOnlineTransactionResponse.Body.StkCallback.MerchantRequestId
                    });
                lipaNaMpesaPayment.MerchantRequestId = linaMpesaOnlineTransactionResponse.Body.StkCallback.MerchantRequestId;
                 lipaNaMpesaPayment.ResultDesc = linaMpesaOnlineTransactionResponse.Body.StkCallback.ResultDesc;
                lipaNaMpesaPayment.CheckoutRequestId = linaMpesaOnlineTransactionResponse.Body.StkCallback.CheckoutRequestId;
                lipaNaMpesaPayment.Amount = Convert.ToDecimal(linaMpesaOnlineTransactionResponse.Body.StkCallback.CallbackMetadata.Item[0].Value);
                lipaNaMpesaPayment.TransactionDate = linaMpesaOnlineTransactionResponse.Body.StkCallback.CallbackMetadata.Item[3].Value;
                lipaNaMpesaPayment.PhoneNumber = linaMpesaOnlineTransactionResponse.Body.StkCallback.CallbackMetadata.Item[4].Value;
                lipaNaMpesaPayment.MpesaReceiptNumber = linaMpesaOnlineTransactionResponse.Body.StkCallback.CallbackMetadata.Item[4].Value;          
              

                this.shoppingCartDbContext.
                              LipaNaMpesaPayments.
                              Update(lipaNaMpesaPayment);
                this.shoppingCartDbContext.SaveChanges();
                return new LipaNaMpesaResponse() {
                    ResultCode = 0,
                    ThirdPartyTransID= "1234567890",
                    ResultDesc= "The service was accepted successfully",
                };
            }
            catch (Exception)
            {
                return new LipaNaMpesaResponse()
                {
                    ResultCode = 9,
                    ThirdPartyTransID = "1234567890",
                    ResultDesc = "The service was not accepted successfully",
                };
            }
          
        }
    }
}
