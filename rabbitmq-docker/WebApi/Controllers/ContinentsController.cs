﻿using Microsoft.AspNetCore.Mvc;
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
        Exchanger.SendMessage(_connectionFactory, message, continent, '.', "continent", ExchangeType.Topic);

        return Ok(temp.GetMessage());
      }
      catch (Exception ex) {
        return BadRequest($"Error in PostContinent: {ex.Message}");
      }
    }
  }
}
