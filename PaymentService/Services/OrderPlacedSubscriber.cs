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
    public class OrderPlacedSubscriber : IOrderPlacedSubscriber
    {
        private ILipaNaMpesaManager _lipaNaMpesaManager;

        public OrderPlacedSubscriber(ILipaNaMpesaManager lipaNaMpesaManager)
        {
            this._lipaNaMpesaManager = lipaNaMpesaManager;
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
            channel.QueueDeclare(queue: "paymentservice",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
            channel.ExchangeDeclare(exchange: "orders", type: "fanout");

            channel.QueueBind(queue: "paymentservice",
                      exchange: "orders",
                      routingKey: "");
            Debug.WriteLine(" [*] Waiting for orders placed paymentsevice");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                CustomerOrder customerOrder = JsonConvert.DeserializeObject<CustomerOrder>(message);
                _lipaNaMpesaManager.MakeMpesaPaymentRequest(customerOrder);
                Debug.WriteLine("Paymentsevive [x] Received {0} ", message);
            };
            channel.BasicConsume(queue: "paymentservice",
                                 autoAck: true,
                                 consumer: consumer);
            //Console.ReadLine();





        }
    }
}
