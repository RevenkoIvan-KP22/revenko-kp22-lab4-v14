using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase, ISender {
        private readonly IConnectionFactory _connectionFactory;
        public OrdersController(IConnectionFactory connectionFactory) {
            _connectionFactory = connectionFactory;
        }

        [HttpGet(Name = "order:new")]
        public async Task<IActionResult> GetNew() {
            return await SendMessage("New order.", "new");
        }

        [HttpGet(Name = "order:new")]
        public async Task<IActionResult> GetProcessing() {
            return await SendMessage("Order is being processed.", "new");
        }

        [HttpGet(Name = "order:new")]
        public async Task<IActionResult> GetComplete() {
            return await SendMessage("Order is complete.", "new");
        }

        [HttpGet(Name = "order:cancelled")]
        public async Task<IActionResult> GetCancelled() {
            return await SendMessage("Order has been cancelled.", "cancelled");
        }

        public async Task<IActionResult> SendMessage(string message, string rKey) {
            try {
                using var connection = _connectionFactory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.ExchangeDeclare(exchange: "order", type: ExchangeType.Direct, durable: true);
                var props = channel.CreateBasicProperties();
                props.Persistent = true;

                string messageToSend = $"Message ID: {new Guid()}\nDate: {DateTime.Now}\nContent: {message}";
                var body = Encoding.UTF8.GetBytes(messageToSend);

                if (rKey == "cancelled") {
                    rKey = "complete";
                }

                channel.BasicPublish(exchange: "order", routingKey: $"order:{rKey}", basicProperties: props, body: body);
                return Ok("Success.");
            }
            catch (Exception ex) {
                return StatusCode(502, ex.Message);
            }
        }
    }
}
