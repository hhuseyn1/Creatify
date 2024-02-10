using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Services.Auth.API.Data;
using Services.Auth.API.Models;
using Services.Auth.API.Models.Dto;
using Services.Auth.API.Services.IAuth;

namespace Services.Auth.API.Services;

public class AuthService : IAuthService
{

    private readonly AppDbContext appDbContext;
    private readonly UserManager<AppUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public AuthService(AppDbContext appDbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        this.appDbContext = appDbContext;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public Task<string> Login(LoginDto loginDto)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> Register(RegisterDto registerDto)
    {
        throw new NotImplementedException();
    }
}
