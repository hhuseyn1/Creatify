using AutoMapper;
using Creatify.MessageBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Services.ShoppingCart.API;
using Services.ShoppingCart.API.Data;
using Services.ShoppingCart.API.Extensions;
using Services.ShoppingCart.API.Service;
using Services.ShoppingCart.API.Service.IService;
using Services.ShoppingCart.API.Utility;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("default"));
});

IMapper mapper = MappingConfig.RegisterMappings().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IMessageBus, MessageBus>();

builder.Services.AddHttpClient("Product", 
    u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddHttpClient("Coupon",
    u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[] {}
        }
    });
});

builder.AddProductServices();

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cart API");
    c.RoutePrefix = string.Empty;
});


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

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