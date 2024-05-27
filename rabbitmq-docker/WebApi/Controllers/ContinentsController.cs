using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers {
  [ApiController]
  [Route("api/[controller]")]
  public class ContinentsController : Controller {
    private readonly IConnectionFactory _connectionFactory;
    private readonly string[] _continents = { "Asia", "Africa", "NA", "SA", "Antarctica", "Europe", "Australia" };

    public ContinentsController(IConnectionFactory connectionFactory) {
      _connectionFactory = connectionFactory;
    }

    [HttpPost]
    public IActionResult PostContinent(string continent, string content) {
      try {
        if (!_continents.Contains(continent)) {
          return NotFound("Tattoine?");
        }
        string message = $"{continent} says \"{content}\"!";
        var temp = new Message();
        temp.Content = content;
        Exchanger.SendMessage(_connectionFactory, message, continent);

        return Ok(temp.GetMessage());
      }
      catch (Exception ex) {
        return BadRequest($"Error in PostContinent: {ex.Message}");
      }
    }

/*    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult SendMessage(string content, string routingKey) {
      try {
        using (var connection = _connectionFactory.CreateConnection()) {
          using (var channel = connection.CreateModel()) {
            var message = new Message();
            message.Content = content;
            var body = Encoding.UTF8.GetBytes(message.GetMessage());
            channel.ExchangeDeclare(exchange: "Continent", type: ExchangeType.Topic);
            channel.BasicPublish(exchange: "Continent", routingKey: $"continent.{routingKey}", body: body);

            return Ok(message.GetMessage());
          }
        }
      }
      catch (Exception ex) {
        return BadRequest($"Error in SendMessage: {ex.Message}");
      }
    }*/
  }
}
