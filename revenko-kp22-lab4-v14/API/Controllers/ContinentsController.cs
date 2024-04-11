using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace API.Controllers {
    public class ContinentsController : ControllerBase, ISender {
        private readonly IConnectionFactory _connectionFactory;
        private string[] _continents = { "Asia", "Africa", "NA", "SA", "Antarctica", "Europe", "Australia" };
        public ContinentsController(IConnectionFactory connectionFactory) {
            _connectionFactory = connectionFactory;
        }

        [HttpGet(Name = "continent.{continent}")]
        public async Task<IActionResult> GetContinent(string continent) {
            try {
                if (!_continents.Contains(continent)) {
                    return NotFound($"You probably live on Mars.");
                }

                string message = String.Concat(_continents[Random.Shared.Next(_continents.Length)], " says: Hello!");

                return await SendMessage(message, continent);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> SendMessage(string message, string rKey) {
            try {
                using var connection = _connectionFactory.CreateConnection();
                using var channel = connection.CreateModel();

                var body = Encoding.UTF8.GetBytes(message);
                string messageToSend = $"Message ID: {new Guid()}\nDate: {DateTime.Now}\nContent: {message}";

                channel.ExchangeDeclare(exchange: "cont", type: ExchangeType.Direct);
                channel.BasicPublish(exchange: "cont", routingKey: $"continent.{rKey}", body: body);
                await Console.Out.WriteLineAsync($"{message} - continent.{rKey}");

                return Ok("Success.");
            }
            catch (Exception ex) {
                return StatusCode(502, ex.Message);
            }
        }
    }
}
