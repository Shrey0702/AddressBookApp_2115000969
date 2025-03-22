using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using Middleware.SMTP;
using BusinessLayer.Interface;

namespace BusinessLayer.Service
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly string _hostname = "localhost";  // Change if needed
        private readonly string _queueName = "AddressBook"; // Queue name
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMQService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void SendMessage(string message)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = _hostname };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] RabbitMQ SendMessage: {ex.Message}");
            }
        }

        public void ReceiveMessage()
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = _hostname };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();

                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    // ✅ Extract email & message
                    var parts = message.Split(',', 2);
                    if (parts.Length == 2)
                    {
                        string email = parts[0].Trim();
                        string emailMessage = parts[1].Trim();

                        // ✅ Use IServiceScopeFactory to resolve IEmailServices
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var smtpService = scope.ServiceProvider.GetRequiredService<IEmailServices>();
                            await smtpService.SendEmailAsync(email, "Welcome to AddressBook", emailMessage);
                        }
                    }
                };

                channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] RabbitMQ ReceiveMessage: {ex.Message}");
            }
        }
    }
}
