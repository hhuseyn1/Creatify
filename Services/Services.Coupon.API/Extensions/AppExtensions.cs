using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Services.Coupon.API.Extensions;

public static class AppExtensions
{
    public static WebApplicationBuilder AddAuthServices(this WebApplicationBuilder builder)
    {
        var apiSettings = builder.Configuration.GetSection("ApiSettings");

        var secret = apiSettings.GetValue<string>("Secret");
        var issuer = apiSettings.GetValue<string>("Isuuer");
        var audience = apiSettings.GetValue<string>("Audience");

        var key = Encoding.ASCII.GetBytes(secret);

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidAudience = audience,
                ValidateAudience = true
            };
        });

        return builder;
    }
}
