using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Receivers {
  class Exe {
    static void Main(string[] args) {
      Utils.ReceiveMessage("complete");
    }
  }
  
}