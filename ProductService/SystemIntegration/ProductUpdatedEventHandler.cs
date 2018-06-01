using Newtonsoft.Json;
using ProductService.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.SystemIntegration
{
    public class ProductUpdatedEventPublisher : IProductUpdatedEventPublisher
    {
        public void Publish(Product updatedProduct)
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
                channel.ExchangeDeclare(exchange: "productexchange", type: "fanout");
                channel.QueueDeclare(queue: "productupdatequeue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
                channel.QueueBind("productupdatequeue", "productexchange", "");

                var message = JsonConvert.SerializeObject(updatedProduct);
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(exchange: "productexchange",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);


            }

        }
    }
}
