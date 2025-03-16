using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class RabbitMQService
    {
        private readonly string _hostname;
        private readonly string _username;
        private readonly string _password;
        private readonly string _exchange;
        private readonly string _queue;
        private readonly string _routingKey;
        // if we want we can also create a separate class for the message model
        // and we can also create an interface for this class
        public RabbitMQService(IConfiguration configuration)
        {
            var rabbitConfig = configuration.GetSection("RabbitMQ");
            _hostname = rabbitConfig["HostName"];
            _username = rabbitConfig["UserName"];
            _password = rabbitConfig["Password"];
            _exchange = rabbitConfig["Exchange"];
            _queue = rabbitConfig["Queue"];
            _routingKey = rabbitConfig["RoutingKey"];
        }

        public void PublishMessage(string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(_exchange, ExchangeType.Direct, durable: true, autoDelete: false);
            channel.QueueDeclare(_queue, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(_queue, _exchange, _routingKey);

            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: _exchange,
                                 routingKey: _routingKey,
                                 basicProperties: properties,
                                 body: body);

            Console.WriteLine($"📢 [Published] {message}");
        }

    }
}
