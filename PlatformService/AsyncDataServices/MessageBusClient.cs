using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PlatformService.Dto;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                // HostName = _configuration.GetValue<string>("RabbitMQHost"),
                // Port = int.Parse(_configuration.GetValue<string>("RabbitMQPort"))
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus {ex.Message}");
            }
        }
        
        
        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> Rabbit MQ Connection Open, sending message...");
                //To do send the message
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> Rabbit MQ Connection Closed, NOT sending message...");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            
            _channel.BasicPublish(exchange: "trigger", routingKey: "", null, body:body);
            Console.WriteLine($"$--> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Dispose");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
        
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Rabbit MQ Connection Shutdown");
        }
    }
}