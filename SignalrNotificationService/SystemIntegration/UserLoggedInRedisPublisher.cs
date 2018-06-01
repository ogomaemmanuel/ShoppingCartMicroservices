using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrNotificationService.SystemIntegration
{
    public class UserLoggedInRedisPublisher : IUserLoggedInRedisPublisher
    {
        
        public UserLoggedInRedisPublisher()
        {

        }
        public  void Publish(UserLoggedInMessage userLoggedInMessage)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri("amqp://guest:guest@rabbitmq:5672");
            connectionFactory.UserName = "guest";
            connectionFactory.Password = "guest";
            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "customerloggin", type: "fanout");
                channel.QueueDeclare(queue: "customerlogginqueue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
                channel.QueueBind("customerlogginqueue", "customerloggin", "");
                
                var message = JsonConvert.SerializeObject(userLoggedInMessage);
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(exchange: "customerloggin",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
                Debug.WriteLine(" [x] Sent {0}", message);


            }
        }
    }
}

public class UserLoggedInMessage{
    public string Channel { get; set; }
    public string Message { get; set; }
    public string UserId { get; set; }
}
