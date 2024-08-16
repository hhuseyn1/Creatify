using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Auth.API.Models;
using Services.Auth.API.Services.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Auth.API.Services
{
	public class JwtTokenGenerator : IJwtTokenGenerator
	{
		private readonly JwtOptions _jwtOptions;
		public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
		{
			_jwtOptions = jwtOptions.Value;
		}
		public string GenerateToken(AppUser appUser, IEnumerable<string> roles)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			var key = Encoding.UTF8.GetBytes(_jwtOptions.Secret);

			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Email,appUser.Email),
				new Claim(JwtRegisteredClaimNames.Sub,appUser.Id),
				new Claim(JwtRegisteredClaimNames.Name,appUser.UserName)

			};

			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Audience = _jwtOptions.Audience,
				IssuedAt = DateTime.UtcNow,
				Issuer = _jwtOptions.Issuer,
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(15),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);

		}
	}
}
