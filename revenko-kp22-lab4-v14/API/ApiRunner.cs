using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IConnectionFactory>(service => {
    try {
        var factory = new ConnectionFactory() {
            HostName = "rabbitmq",
            Port = 5672,
            UserName = "jason",
            Password = "bourne",
        };
        factory.Uri = new Uri("amqp://jason:bourne@rabbitmq:5672");
        return factory;
    }
    catch (Exception ex){
        Console.WriteLine($"Error occured while creating the connection factory: {ex}");
        throw;
    }
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
