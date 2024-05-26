using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string HOSTNAME = "rabbitmq";
const int PORT = 5672;
const string USERNAME = "jason";
const string PASSWORD = "bourne";
builder.Services.AddSingleton<IConnectionFactory>(x => {
  try {
    var factory = new ConnectionFactory() {
      HostName = HOSTNAME,
      Port = PORT,
      UserName = USERNAME,
      Password = PASSWORD,
      Uri = new Uri($"amqp://{USERNAME}:{PASSWORD}@{HOSTNAME}:{PORT}/"),
    };
    return factory;
  }
  catch (Exception ex) {
    Console.WriteLine($"Error during the creation of RabbitMQ connection: {ex.Message}");
    throw;
  }
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
