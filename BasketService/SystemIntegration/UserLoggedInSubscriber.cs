using BasketService.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketService.SystemIntegration
{
    public class UserLoggedInSubscriber 
    {
        private IBasketChangedNotificationSender _basketChangedNotificationSender;
        public UserLoggedInSubscriber(IBasketChangedNotificationSender basketChangedNotificationSender)
            
        {
            _basketChangedNotificationSender = basketChangedNotificationSender;


        }
        public void Subscribe()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri("amqp://guest:guest@rabbitmq:5672");
            connectionFactory.UserName = "guest";
            connectionFactory.Password = "guest";
            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            IConnection connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "customerlogginqueue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                  arguments: null);
            channel.ExchangeDeclare(exchange: "customerloggin", type: "fanout");
            
           

            channel.QueueBind(queue: "customerlogginqueue",
                      exchange: "customerloggin",
                      routingKey: "");
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                UserLoggedInMessage customerOrder = JsonConvert.DeserializeObject<UserLoggedInMessage>(message);
                var customerBasketTotal = 0; //_repository.Count(customerOrder.UserId).ToString();
                _basketChangedNotificationSender.PublishCustomerBasketTotal(customerOrder.UserId, customerBasketTotal.ToString());
                //channel.BasicAck(ea.DeliveryTag, true);
            };
            channel.BasicConsume(queue: "customerlogginqueue",
                                 autoAck: true,
                                 consumer: consumer);

        }
    }

    public class UserLoggedInMessage
    {
        public string Channel { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
    }
}
