using Creatify.Shared.Models;

namespace Services.Auth.API.Services.IService;

public interface IJwtTokenGenerator
{
	string GenerateToken(AppUser appUser, IEnumerable<string> roles);
}
