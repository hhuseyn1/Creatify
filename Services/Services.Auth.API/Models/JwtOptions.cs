namespace Services.Auth.API.Models;

public class JwtOptions
{
	public string Isuuer { get; set; } = string.Empty;
	public string Audience { get; set; } = string.Empty;
	public string Secret { get; set; } = string.Empty;
}
