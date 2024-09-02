using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Creatify.Gateway.Extensions;

public static class AppExtensions
{
	public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
	{
		var apiSettings = builder.Configuration.GetSection("ApiSettings");

		var issuer = apiSettings.GetValue<string>("Issuer");
		var audience = apiSettings.GetValue<string>("Audience");
		var secret = apiSettings.GetValue<string>("Secret");

		var key = Encoding.ASCII.GetBytes(secret);

		builder.Services.AddAuthentication(x =>
		{
			x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(x =>
		{
			x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
			{
				ValidIssuer = issuer,
				ValidAudience = audience,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateIssuerSigningKey = true
			};
		});

		return builder;
	}
}
