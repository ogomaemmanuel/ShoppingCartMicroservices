using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SignalrNotificationService.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrNotificationService.SystemIntegration
{
    public class ProductUpdateSubscriber : IProductUpdateSubscriber
    {
        private IHubContext<NotificationHub, INotificationHub> _hubContext;
        public ProductUpdateSubscriber(IHubContext<NotificationHub, INotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public void Subscribe() {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri("amqp://guest:guest@rabbitmq:5672");
            connectionFactory.UserName = "guest";
            connectionFactory.Password = "guest";
            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            IConnection connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "notificationhubqueue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
            channel.ExchangeDeclare(exchange: "productexchange", type: "fanout");

            channel.QueueBind(queue: "productupdatequeue",
                      exchange: "productexchange",
                      routingKey: "");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Product updatedProduct = JsonConvert.DeserializeObject<Product>(message);
                    this._hubContext.Clients.All.SendToAll("productChanged",message);
            };
            channel.BasicConsume(queue: "productupdatequeue",
                                 autoAck: true,
                                 consumer: consumer);
        }

    }

    public class Product
    { 
        public string ProductName { get; set; }
        public string ProductMediaFile { get; set; }
        public string ProductSku { get; set; }
        public Guid ProductCategory { get; set; }
        public string ProductManufacturer { get; set; }
        public decimal Price { get; set; }
        public int ShopperReview { get; set; }
    }
}

