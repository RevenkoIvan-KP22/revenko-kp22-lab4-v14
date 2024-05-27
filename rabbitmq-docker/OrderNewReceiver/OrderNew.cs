using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Receivers {
  class Exe {
    static void Main(string[] args) {
      Utils.ReceiveMessage("new");
    }
  }

  public class Utils { 
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

      Console.WriteLine($"order:{rkey} receiver:");

      string queue = $"Order:{rkey}";

      try {
        using (var connection = factory.CreateConnection()) {
          using (var channel = connection.CreateModel()) {
            channel.ExchangeDeclare("order", ExchangeType.Direct);
            channel.QueueDeclare(queue, true, false, false);
            channel.QueueBind(queue: queue, exchange: "order", routingKey: $"order:{rkey}");
            if (rkey == "complete") {  
              channel.QueueBind(queue: queue, exchange: "order", routingKey: "order:cancelled");
            }

            var Consumer = new EventingBasicConsumer(channel);
            Consumer.Received += (sender, args) => {
              var body = args.Body.ToArray();
              var message = Encoding.UTF8.GetString(body);
              Console.WriteLine(message + "\n" + $"This has been consumed by {queue}");
            };

            channel.BasicConsume(queue, true, Consumer);
            Console.ReadLine();
          }
        }
      }
      catch (Exception ex) {
        Console.WriteLine($"Error in {rkey} Consumer: {ex.Message}");
      }
    }
  }
}