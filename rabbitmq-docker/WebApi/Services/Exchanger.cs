using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using WebApi.Models;

namespace WebApi.Services {
  public class Exchanger {
    public static void SendMessage(IConnectionFactory factory, string content, string routingKey, char @char, string exchange, string exchangeType) {
      try {
        using (var connection = factory.CreateConnection()) {
          using (var channel = connection.CreateModel()) {
            var message = new Message();
            message.Content = content;
            var body = Encoding.UTF8.GetBytes(message.GetMessage());
            channel.ExchangeDeclare(exchange: exchange, type: exchangeType);
            channel.BasicPublish(exchange: exchange, routingKey: $"{exchange}{@char}{routingKey}", body: body);
          }
        }
      }
      catch (Exception ex) {
        Console.WriteLine($"Error in SendMessage: {ex.Message}");
      }
    }
  }
}
