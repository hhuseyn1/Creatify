using Microsoft.EntityFrameworkCore;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using Services.Reward.API.Data;
using Services.Reward.API.Extension;
using Services.Reward.API.Messaging;
using Services.Reward.API.Services;
using Services.Reward.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .Enrich.WithProperty("ServiceName", "RewardAPI")
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.json", rollingInterval: RollingInterval.Day)
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "reward-api-logs-{0:yyyy.MM.dd}"
    })
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("default"));
});

var myAllowSpecificOrigins = "GatewayAPICorsOrigin";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7777")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                      });
});

var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("default"));
builder.Services.AddSingleton(new RewardService(optionBuilder.Options));

builder.Services.AddSingleton<IAzureServiceBusConsumer,AzureServiceBusConsumer>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reward API");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseGlobalException();

app.MapControllers();

app.UseCors(myAllowSpecificOrigins);
ApplyMigration();
app.UseAzureServiceBusConsumer();
app.Run();
void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
            _db.Database.Migrate();
    }
}