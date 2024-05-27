using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Receivers {
  class Execution {
    static void Main(string[] args) {
      Utils.ReceiveMessage();
      Console.ReadLine();
    }
  }

  public class Utils { 
    public static void ReceiveMessage() {
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

      Console.WriteLine($"Continent receiver:");
      string queue = $"Continent-Queue";

      try {
        using (var connection = factory.CreateConnection()) {
          using (var channel = connection.CreateModel()) {
            channel.ExchangeDeclare("Continent", ExchangeType.Topic);
            channel.QueueDeclare(queue, true, false, false);
            channel.QueueBind(queue: queue, exchange: "Continent", routingKey: $"continent.#");

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
        Console.WriteLine($"Error in Continent Consumer: {ex.Message}");
      }
    }
  }
}