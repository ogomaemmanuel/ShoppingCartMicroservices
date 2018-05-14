namespace BasketService.Services
{
    public interface IBasketChangedNotificationSender
    {
        void PublishCustomerBasketTotal(string groupId, string message);
    }
}