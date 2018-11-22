using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

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
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Reived msg, start to simulate execution...");
                    foreach (var i in message.ToCharArray())
                    {
                        Thread.Sleep(150);
                        Console.Write("{0}", i);
                    }

                    //If auto ack is false, then need to send ack manually.
                    #region Manual send acknowledgement
                    //Console.WriteLine("Msg handle finished, send acknowledgement now");
                    //channel.BasicAck(ea.DeliveryTag, false);
                    #endregion Manual send acknowledgement

                    Console.WriteLine();
                    
                };
                channel.BasicConsume(queue: "hello",
                                     autoAck: false,    //true: auto ack; false: should send back the ack or the msg can't be removed from queue.
                                     consumer: consumer);
                Console.WriteLine("Press any to exit.");
                Console.ReadKey();
            }
            
        }
    }
}
