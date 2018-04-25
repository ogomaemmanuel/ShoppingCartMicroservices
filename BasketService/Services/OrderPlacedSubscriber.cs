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
    public class OrderPlacedSubscriber : IOrderPlacedSubsriber
    {
        private readonly IRepository<BasketItem> _basketManager;
        public OrderPlacedSubscriber(IRepository<BasketItem> basketManager)
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

            //ConnectionFactory connectionFactory = new ConnectionFactory();
            //connectionFactory.Uri = new Uri("amqp://icufoydf:2M59Ck8mFVEkENoA0ArJv3a5fymlSDxW@spider.rmq.cloudamqp.com/icufoydf");
            //connectionFactory.UserName = "icufoydf";
            //connectionFactory.Password = "2M59Ck8mFVEkENoA0ArJv3a5fymlSDxW";
            //connectionFactory.AutomaticRecoveryEnabled = true;
            // connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);


            using (IConnection connection = connectionFactory.CreateConnection())
            {

                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "orderplaced",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        CustomerOrder customerOrder = JsonConvert.DeserializeObject<CustomerOrder>(message);
                        _basketManager.ClearBasketByCustomerId(customerOrder.CustomerId);
                        Debug.WriteLine(" [x] Received {0}", message);
                    };
                    channel.BasicConsume(queue: "orderplaced",
                                         autoAck: true,
                                         consumer: consumer);
                }

            }
        }
    }
}
