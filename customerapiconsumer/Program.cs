using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace customerapiconsumer
{
    class Program
    {

        static Program()
        {
            
        }
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName  = "localhost",
                UserName = "admin",
                Password = "123456"
            };

            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "customer",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var jsonString = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Json receievd as {jsonString}");
            };

            channel.BasicConsume(
                queue: "customer",
                autoAck: true,
                consumer: consumer
            );

            Console.ReadLine();
        }
    }
}
