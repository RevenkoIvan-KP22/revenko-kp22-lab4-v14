using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Receivers {
  class Exe {
    static void Main(string[] args) {
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

      try {
        using (var connection = factory.CreateConnection()) {
          using (var channel = connection.CreateModel()) {
            channel.ExchangeDeclare("Continent", ExchangeType.Direct);
            channel.QueueDeclare("Continent-Queue", true, false, false);
            channel.QueueBind(queue: "Continent-Queue", exchange: "Continent", routingKey: "continent.Africa");

            var Consumer = new EventingBasicConsumer(channel);
            Consumer.Received += (sender, args) => {
              var body = args.Body.ToArray();
              var message = Encoding.UTF8.GetString(body);
              Console.WriteLine(message);
            };

            channel.BasicConsume("Continent-Queue", true, Consumer);
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