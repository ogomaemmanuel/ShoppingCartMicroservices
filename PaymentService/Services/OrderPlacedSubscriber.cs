using Newtonsoft.Json;
using PaymentService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Services
{
    public class OrderPlacedSubscriber
    {
        private ILipaNaMpesaManager _lipaNaMpesaManager;

        public OrderPlacedSubscriber(ILipaNaMpesaManager lipaNaMpesaManager)
        {
            this._lipaNaMpesaManager = lipaNaMpesaManager;
        }

        public void Handle() {
          

            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri("amqp://guest:guest@rabbitmq:5672");
            connectionFactory.UserName = "guest";
            connectionFactory.Password = "guest";
            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);  
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
                        _lipaNaMpesaManager.MakeMpesaPaymentRequest(customerOrder);
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
