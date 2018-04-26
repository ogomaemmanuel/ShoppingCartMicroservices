namespace PaymentService.Services
{
    public interface IOrderPlacedSubscriber
    {
        void Handle();
    }
}