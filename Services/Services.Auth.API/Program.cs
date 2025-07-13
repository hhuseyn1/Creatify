using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Auth.API.Data;
using Services.Auth.API.Models;
using Services.Auth.API.RabbitMQSender;
using Services.Auth.API.Services;
using Services.Auth.API.Services.IAuth;
using Services.Auth.API.Services.IService;
using Serilog;
using Services.Auth.API.Extensions;
using Serilog.Sinks.Elasticsearch;
using Creatify.Shared;

var builder = WebApplication.CreateBuilder(args);

// Configure additional configuration files
builder.Configuration
.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
.AddJsonFile("appsettings.AuthAPI.json", optional: true, reloadOnChange: true)
.AddJsonFile($"appsettings.AuthAPI.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .Enrich.WithProperty("ServiceName", "AuthAPI")
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.json", rollingInterval: RollingInterval.Day)
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "auth-api-logs-{0:yyyy.MM.dd}"
    })
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRabbitMQAuthMessageSender, RabbitMQAuthMessageSender>();

builder.Services.AddValidatorsFromAssemblyContaining<Creatify.Web.Validators.LoginDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<Creatify.Web.Validators.RegisterDtoValidator>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseGlobalException();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors(myAllowSpecificOrigins);

ApplyMigration();

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