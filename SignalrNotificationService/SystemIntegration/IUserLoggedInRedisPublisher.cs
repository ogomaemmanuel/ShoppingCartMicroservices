using System.Threading.Tasks;

namespace SignalrNotificationService.SystemIntegration
{
    public interface IUserLoggedInRedisPublisher
    {
        void Publish(UserLoggedInMessage userLoggedInMessage);
    }
}