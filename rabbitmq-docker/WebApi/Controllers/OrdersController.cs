using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using WebApi.Controllers;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers {
  [ApiController]
  [Route("api/[controller]")]
  public class OrdersController : Controller {
    private readonly IConnectionFactory _connectionFactory;
    private readonly string[] _orders = { "new", "processing", "complete", "cancelled" };
    public OrdersController(IConnectionFactory connectionFactory) {
      _connectionFactory = connectionFactory;
    }

    [HttpPost]
    public IActionResult PostOrder(string order, string content) {
      try {
        if (!_orders.Contains(order)) {
          return NotFound("No.");
        }

        string message = $"New order of the type {order}.\n\"{content}\"!";
        var temp = new Message();
        temp.Content = content;
        Exchanger.SendMessage(_connectionFactory, message, order, ':', "order", ExchangeType.Direct);

        return Ok(temp.GetMessage());
      }
      catch (Exception ex) {
        return BadRequest($"Error in PostOrder: {ex.Message}");
      }
    }

    private static void SendMessage(IConnectionFactory factory, string content, string routingKey) {
      try {
        using (var connection = factory.CreateConnection()) {
          using (var channel = connection.CreateModel()) {
            var message = new Message();
            message.Content = content;
            var body = Encoding.UTF8.GetBytes(message.GetMessage());
            channel.ExchangeDeclare(exchange: $"Order", type: ExchangeType.Direct);
            channel.BasicPublish(exchange: $"Order", routingKey: $"order:{routingKey}", body: body);
          }
        }
      }
      catch (Exception ex) {
        Console.WriteLine($"Error in SendMessage: {ex.Message}");
      }
    }
  }
}
