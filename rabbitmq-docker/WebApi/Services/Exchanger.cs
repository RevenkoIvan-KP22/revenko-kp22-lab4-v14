using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using WebApi.Models;

namespace WebApi.Services {
  public class Exchanger {
    public static void SendMessage(IConnectionFactory factory, string content, string routingKey) {
      try {
        using (var connection = factory.CreateConnection()) {
          using (var channel = connection.CreateModel()) {
            var message = new Message();
            message.Content = content;
            var body = Encoding.UTF8.GetBytes(message.GetMessage());
            channel.ExchangeDeclare(exchange: "Continent", type: ExchangeType.Topic);
            channel.BasicPublish(exchange: "Continent", routingKey: $"continent.{routingKey}", body: body);
          }
        }
      }
      catch (Exception ex) {
        Console.WriteLine($"Error in SendMessage: {ex.Message}");
      }
    }

    public static void ReceiveMessage(string rkey) {
      const string HOSTNAME = "rabbitmq";
      const int PORT = 5672;
      const string USERNAME = "jason";
      const string PASSWORD = "bourne";

      var factory = new ConnectionFactory() {
        HostName = HOSTNAME,
        Port = PORT,
        UserName = USERNAME,
        Password = PASSWORD,
        Uri = new Uri($"amqp://{USERNAME}:{PASSWORD}@{HOSTNAME}:{PORT}/"),
      };

      Console.WriteLine("Africa receiver:");
      string queue = $"Continent-Queue-{rkey}";

      try {
        using (var connection = factory.CreateConnection()) {
          using (var channel = connection.CreateModel()) {
            channel.ExchangeDeclare("Continent", ExchangeType.Topic);
            channel.QueueDeclare(queue, true, false, false);
            channel.QueueBind(queue: queue, exchange: "Continent", routingKey: $"continent.{rkey}");

            var Consumer = new EventingBasicConsumer(channel);
            Consumer.Received += (sender, args) => {
              var body = args.Body.ToArray();
              var message = Encoding.UTF8.GetString(body);
              Console.WriteLine(message);
            };

            channel.BasicConsume(queue, true, Consumer);
            Console.ReadLine();
          }
        }
      }
      catch (Exception ex) {
        Console.WriteLine($"Error in NA Consumer: {ex.Message}");
      }
    }
  }
}
