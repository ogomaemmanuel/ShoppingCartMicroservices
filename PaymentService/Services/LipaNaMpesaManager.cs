using Microsoft.Extensions.Options;
using PaymentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using System.Diagnostics;
using Newtonsoft.Json;

namespace PaymentService.Services
{
    public class LipaNaMpesaManager : ILipaNaMpesaManager
    {
        private readonly IOptions<StkSetting> _stkSettings;
        private readonly IOptions<ShoppingCartStkPushKey> _shoppingCartStkPushKey;
        private readonly Func<ShoppingCartDbContext> shoppingCartDbContext;
        public LipaNaMpesaManager(Func<ShoppingCartDbContext> shoppingCartDbContext, IOptions<StkSetting> stkSettings, IOptions<ShoppingCartStkPushKey> shoppingCartStkPushKey)
        {
            this.shoppingCartDbContext = shoppingCartDbContext;
            _stkSettings = stkSettings;
            _shoppingCartStkPushKey = shoppingCartStkPushKey;
        }

        public void MakeMpesaPaymentRequest(CustomerOrder customerOrder)
        {
            using (var context = this.shoppingCartDbContext()) {
                LipaNaMpesaPayment lipaNaMpesaPaymentRequest = JsonConvert.DeserializeObject<LipaNaMpesaPayment>(SendStkPushNotifaction());
                context.LipaNaMpesaPayments.Add(lipaNaMpesaPaymentRequest);
                context.SaveChanges();
            }
                          
        }

        private void GetMpesaAuthenticationToken() { }

        public LipaNaMpesaResponse Save(LinaMpesaOnlineTransactionResponse
            linaMpesaOnlineTransactionResponse)
        {
            try
            {
                using (var context = this.shoppingCartDbContext())
                {
                    var lipaNaMpesaPayment= context.
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

                

                    context.LipaNaMpesaPayments.
                    Update(lipaNaMpesaPayment);
                    context.SaveChanges();
                    return new LipaNaMpesaResponse()
                    {
                        ResultCode = 0,
                        ThirdPartyTransID = "1234567890",
                        ResultDesc = "The service was accepted successfully",
                    };
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

        private string SendStkPushNotifaction()
        {


            ShoppingCartApiAccessToken shoppingCartApiAccessToken = GetAuthToken();
            var result = "https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest"
                .WithOAuthBearerToken(shoppingCartApiAccessToken.AccessToken)
                .PostJsonAsync(this._stkSettings.Value)
                .ReceiveString().Result;
            return result;
        }


        private ShoppingCartApiAccessToken GetAuthToken()
        {

            var result = _shoppingCartStkPushKey.Value.Url
                .WithBasicAuth(_shoppingCartStkPushKey.Value.ConsumerKey, _shoppingCartStkPushKey.Value.ConsumerSecret)
                .GetStringAsync().Result;
            ShoppingCartApiAccessToken shoppingCartApiAccessToken = JsonConvert.DeserializeObject<ShoppingCartApiAccessToken>(result);
            Debug.Write(JsonConvert.SerializeObject(this._stkSettings.Value));
            return shoppingCartApiAccessToken;
        }
    }
}
