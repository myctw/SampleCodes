using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace Producer
{
    class Producer
    {
        static void Main(string[] args)
        {
            //Remember to add "admin" account to RabbitMQ or replace to other account
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "admin", Password = "pass123" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //type: direct, topic, fanout, and headers
                    channel.ExchangeDeclare(exchange: "logs", type:"fanout");
                    
                    var message = $"Hello RabbitMQ. {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}";
                    var body = Encoding.UTF8.GetBytes(message);
                    
                    channel.BasicPublish(exchange: "logs",
                                         routingKey: "",
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine($"Sent msg [{message}] to Exchange");
                    Thread.Sleep(3000);
                }
            }
        }
    }
}
