using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

namespace Subscriber
{
    class Subscriber
    {
        static void Main(string[] args)
        {
            //Remember to add "admin" account to RabbitMQ or replace to other account
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "admin", Password = "pass123" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: "fanout");
                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                                  exchange: "logs",
                                  routingKey: "");
                Console.WriteLine("Subscriber is ready to listen queue.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Reived msg, start to simulate execution...");
                    foreach (var i in message.ToCharArray())
                    {
                        Thread.Sleep(50);
                        Console.Write("{0}", i);
                    }
                    Console.WriteLine();
                    
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,    //true: auto ack; false: should send back the ack or the msg can't be removed from queue.
                                     consumer: consumer);
                Console.WriteLine("Press any to exit.");
                Console.ReadKey();
            }
            
        }
    }
}
