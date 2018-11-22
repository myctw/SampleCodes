using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receiver
{
    class Receiver
    {
        static void Main(string[] args)
        {
            //Remember to add "admin" account to RabbitMQ or replace to other account
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "admin", Password = "pass123" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("Received {0}", message);
                };
                channel.BasicConsume(queue: "hello",
                                     autoAck: true,
                                     consumer: consumer);
                Console.WriteLine("Press any to exit.");
                Console.ReadKey();
            }
            
        }
    }
}
