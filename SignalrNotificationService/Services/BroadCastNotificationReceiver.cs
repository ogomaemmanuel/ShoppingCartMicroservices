using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SignalrNotificationService.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrNotificationService.Services
{
    public class BroadCastNotificationReceiver: IBroadCastNotificationReceiver
    {
        private IHubContext<NotificationHub, INotificationHub> _hubContext;

        public BroadCastNotificationReceiver(IHubContext<NotificationHub, INotificationHub> hubContext)
        {
            this._hubContext = hubContext;
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
            channel.QueueDeclare(queue: "notificationhubqueue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
            channel.ExchangeDeclare(exchange: "notificationhub", type: "fanout");

            channel.QueueBind(queue: "notificationhubqueue",
                      exchange: "notificationhub",
                      routingKey: "");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                NotificationMessage notificationMessage = JsonConvert.DeserializeObject<NotificationMessage>(message);
                if (notificationMessage.GroupId.ToLower() != "all")
                {
                    this._hubContext.Clients.Group(notificationMessage.GroupId).SendToAll(messageType: notificationMessage.MessageType, message: notificationMessage.Message);
                }
                else {
                    this._hubContext.Clients.All.SendToAll(messageType: notificationMessage.MessageType, message: notificationMessage.MessageType);
                }
                
            };
            channel.BasicConsume(queue: "notificationhubqueue",
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
public class NotificationMessage
{
    public string GroupId { get; set; }
    public string Message { get; set; }
    public string MessageType { get; set; }
}
