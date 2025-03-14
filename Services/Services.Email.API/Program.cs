using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Services.Email.API.Data;
using Services.Email.API.Extension;
using Services.Email.API.Messaging;
using Services.Email.API.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) Controller və Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2) DbContext-in DI-ya əlavə olunması (scoped)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("default")));

// 3) EmailService yalnız scoped kimi qeydiyyatdan keçirin
//    (singletonla manual qaydada yaratmağı ləğv edirik)
builder.Services.AddScoped<IEmailService, EmailService>();

// 4) Azure Service Bus Consumer (singleton)
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

// 5) RabbitMQ IConnection qeydiyyatı (singleton)
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory
    {
        HostName = builder.Configuration["RabbitMQ:HostName"] ?? "localhost",
        UserName = builder.Configuration["RabbitMQ:UserName"] ?? "guest",
        Password = builder.Configuration["RabbitMQ:Password"] ?? "guest"
    };
    return factory.CreateConnection();
});

// 6) RabbitMQ Hosted Services (singleton) 
//    * Əgər DbContext və ya IEmailService kimi scoped obyektlərə ehtiyac varsa, 
//      konstruktorda IServiceScopeFactory istifadə edin.
builder.Services.AddHostedService<RabbitMQOrderConsumer>();
builder.Services.AddHostedService<RabbitMQAuthConsumer>();
builder.Services.AddHostedService<RabbitMQCartConsumer>();

var app = builder.Build();

// 7) Swagger konfiqurasiyası
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Email API");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// 8) Migration tətbiqi
ApplyMigration();

// 9) Azure Service Bus Consumer işlədilməsi
app.UseAzureServiceBusConsumer();

app.Run();

// Metod: Bazada pending migration-ları tətbiq edir
void ApplyMigration()
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (db.Database.GetPendingMigrations().Any())
        db.Database.Migrate();
}
