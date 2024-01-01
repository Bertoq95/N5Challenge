using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using N5Challenge.Application;
using N5Challenge.Infrastructure.EntityFramework;
using Nest;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() 
    .CreateLogger();

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("N5Connection"));
});

builder.Services.AddSingleton(provider =>
{
    var config = builder.Configuration.GetSection("ElasticsearchConfig");
    var clusterUrl = config.GetValue<string>("ClusterUrl");

    var settings = new ConnectionSettings(new Uri(clusterUrl));

    return new ElasticClient(settings);
});



builder.Services.AddSingleton<IProducer<string, string>>(provider =>
{
    var config = builder.Configuration.GetSection("KafkaConfig");
    var bootstrapServers = config.GetValue<string>("BootstrapServers");

    var producerConfig = new Confluent.Kafka.ProducerConfig
    {
        BootstrapServers = bootstrapServers
    };

    return new ProducerBuilder<string, string>(producerConfig).Build();
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
