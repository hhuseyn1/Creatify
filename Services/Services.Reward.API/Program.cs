using Microsoft.EntityFrameworkCore;
using Services.Reward.API.Data;
using Services.Reward.API.Extension;
using Services.Reward.API.Messaging;
using Services.Reward.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("default"));
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

app.MapControllers();

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