using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketService.Services
{
    public class BasketChangedNotificationSender : IBasketChangedNotificationSender
    {

        public void PublishCustomerBasketTotal(string groupId, string basketTotal)
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
                channel.ExchangeDeclare(exchange: "notificationhub", type: "fanout");
                channel.QueueDeclare(queue: "notificationhubqueue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

                channel.QueueBind("notificationhubqueue", "notificationhub", "");

                var message =
                    JsonConvert.SerializeObject(
                        new NotificationMessage
                        {
                            GroupId = groupId,
                            Message = basketTotal,
                            MessageType = "BasketChanged"
                        });
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(exchange: "notificationhub",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
                Debug.WriteLine(" [x] Sent {0}", message);
            }
        }
    }

    public class NotificationMessage
    {
        public string GroupId { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }

    }
}
