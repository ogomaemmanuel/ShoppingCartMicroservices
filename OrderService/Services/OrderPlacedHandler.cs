using Newtonsoft.Json;
using OrderService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Services
{
    public class OrderPlacedHandler
    {


        public static void PublishOrderPlaced(CustomerOrder customerOrder) {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri("amqp://guest:guest@rabbitmq:5672");
            connectionFactory.UserName = "guest";
            connectionFactory.Password = "guest";
            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "orders", type: "fanout");
                channel.QueueDeclare(queue: "basketservice",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
                channel.QueueDeclare(queue: "paymentservice",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
                channel.QueueBind("basketservice", "orders", "");
                channel.QueueBind("paymentservice", "orders","");
                var message = JsonConvert.SerializeObject(customerOrder);
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;


                channel.BasicPublish(exchange: "orders",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
                Debug.WriteLine(" [x] Sent {0}", message);
            }

        }
    }
}
