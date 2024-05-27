using RabbitMQ.Client;

namespace WebApi.Models {
  public class Message {
    public DateTime Created { get; private set; }
    public Guid Id { get; private set; }

    public Message(){
      Created = DateTime.Now;
      Id = Guid.NewGuid();
    }

    public string Content { get; set; } 

    public string GetMessage() {
      return $"Created at: {Created}\nId: {Id}\nMessage: {Content}";
    }
  }
}
