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

        public static void publishOrderPlaced(CustomerOrder customerOrder) {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "orderplaced",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var message = JsonConvert.SerializeObject(customerOrder);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "orderplaced",
                                     basicProperties: null,
                                     body: body);
                Debug.WriteLine(" [x] Sent {0}", message);
            }

        }
    }
}
