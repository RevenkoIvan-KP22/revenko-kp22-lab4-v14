﻿using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class OrdersProgram {
    static void Main(string[] args) {

        if (args.Length == 0) {
            Console.WriteLine("Empty args.");
            return;
        }

        string rkey = args[0];

        if (rkey == "cancelled") {
            rkey = "complete";
        }

        try {
            var factory = new ConnectionFactory() {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "jason",
                Password = "bourne",
            };
            factory.Uri = new Uri("amqp://jason:bourne@rabbitmq:5672/");

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel()) {
                channel.ExchangeDeclare(exchange: "order", type: ExchangeType.Direct);
                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName, exchange: "order", routingKey: $"order:{rkey}");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, eventArgs) => {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Received message with routing key {eventArgs.RoutingKey}:\n" + message);

                    channel.BasicAck(eventArgs.DeliveryTag, false);
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                while (true) { }
            }

        }
        catch (Exception ex) {
            Console.WriteLine($"Error occured during the runtime: {ex}");
        }
    }



}