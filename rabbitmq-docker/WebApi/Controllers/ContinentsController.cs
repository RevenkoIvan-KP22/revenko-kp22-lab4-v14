using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

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
    public IActionResult PostContinent(string continent) {
      try {
        if (!_continents.Contains(continent)) {
          return NotFound("Tattoine?");
        }

        string message = $"{continent} says chipi-chipi chapa-chapa!";
        return SendMessage(message, continent);
      }
      catch (Exception ex) {
        return BadRequest($"Error in PostContinent: {ex.Message}");
      }
    }

    private IActionResult SendMessage(string message, string routingKey) {
      try {
        using (var connection = _connectionFactory.CreateConnection()) {
          using (var channel = connection.CreateModel()) {
            var body = Encoding.UTF8.GetBytes(message);
            channel.ExchangeDeclare(exchange: "Continent", type: ExchangeType.Direct);
            channel.BasicPublish(exchange: "Continent", routingKey: $"continent.{routingKey}", body: body);

            return Ok($"Message: {message}");
          }
        }
      }
      catch (Exception ex) {
        return BadRequest($"Error in SendMessage: {ex.Message}");
      }
    }
  }
}
