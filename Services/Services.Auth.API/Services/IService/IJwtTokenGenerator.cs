using Services.Auth.API.Models;

namespace Services.Auth.API.Services.IService;

public interface IJwtTokenGenerator
{
    string GenerateToken(AppUser appUser, IEnumerable<string> roles);
}
