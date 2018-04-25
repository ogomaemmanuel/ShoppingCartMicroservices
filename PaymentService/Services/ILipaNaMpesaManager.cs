using PaymentService.Models;

namespace PaymentService.Services
{
    public interface ILipaNaMpesaManager
    {
        LipaNaMpesaResponse Save(LinaMpesaOnlineTransactionResponse linaMpesaOnlineTransactionResponse);
        void MakeMpesaPaymentRequest(CustomerOrder customerOrder);
    }
}