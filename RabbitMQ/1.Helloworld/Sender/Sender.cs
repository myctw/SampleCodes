using RabbitMQ.Client;
using System;
using System.Text;

namespace Sender
{
    class Sender
    {
        static void Main(string[] args)
        {
            //Remember to add "admin" account to RabbitMQ or replace to other account
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "admin", Password = "pass123" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                    var message = $"Hello RabbitMQ. {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "hello",
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine($"Sent msg [{message}] to Queue");
                    Console.ReadKey();
                }
            }
        }
    }
}
