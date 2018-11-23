using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Subscriber
{
    class Subscriber
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");
                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName, exchange: "direct_logs", routingKey: "INFO");
                channel.QueueBind(queue: queueName, exchange: "direct_logs", routingKey: "WARNING");
                channel.QueueBind(queue: queueName, exchange: "direct_logs", routingKey: "ERROR");
                channel.QueueBind(queue: queueName, exchange: "direct_logs", routingKey: "FATAL");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var msg = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine($"Receiving RoutingKey:{routingKey}, Msg:{msg}.");
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press any to exit.");
                Console.ReadLine();
            }
        }
    }
}
