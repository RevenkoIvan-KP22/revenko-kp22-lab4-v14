using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string HOSTNAME = "localhost";
const int PORT = 5672;
const string USERNAME = "guest";
const string PASSWORD = "guest";
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
