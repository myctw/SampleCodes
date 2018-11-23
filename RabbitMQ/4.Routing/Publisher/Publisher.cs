using System;
using RabbitMQ.Client;
using System.Text;

namespace Publisher
{
    class Publisher
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");
                var msg_error = $"This is Error msg.";
                var msg_info = $"This is Info msg";

                channel.BasicPublish(exchange: "direct_logs",
                    routingKey: "ERROR",
                    basicProperties: null,
                    body: Encoding.UTF8.GetBytes(msg_error));
                channel.BasicPublish(exchange: "direct_logs",
                    routingKey: "INFO",
                    basicProperties: null,
                    body: Encoding.UTF8.GetBytes(msg_info));


            }
        }
    }
}
