using BasketService.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketService.Services
{

    public class BasketChangedHandler : IBasketChangedHandler
    {
        private readonly IRepository<BasketItem> _basketManager;
        public BasketChangedHandler(IRepository<BasketItem> basketManager)
        {
            _basketManager = basketManager;
        }

        public void Handle()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri("amqp://guest:guest@rabbitmq:5672");
            connectionFactory.UserName = "guest";
            connectionFactory.Password = "guest";
            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            IConnection connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "basketservice",
                durable: true,
           exclusive: false,
           autoDelete: false,
           arguments: null);
            channel.ExchangeDeclare(exchange: "orders", type: "fanout");

            channel.QueueBind(queue: "basketservice",
                      exchange: "orders",
                      routingKey: "");
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                CustomerOrder customerOrder = JsonConvert.DeserializeObject<CustomerOrder>(message);
                _basketManager.ClearBasketByCustomerId(customerOrder.CustomerId);
                Debug.WriteLine("BasketSevice [x] Received {0}", message);
            };
            channel.BasicConsume(queue: "basketservice",
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
