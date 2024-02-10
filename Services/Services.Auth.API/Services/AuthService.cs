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

    public async Task<string> Register(RegisterDto registerDto)
    {
        AppUser appUser = new()
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            NormalizedEmail = registerDto.Email.ToUpper(),
            PhoneNumber = registerDto.PhoneNumber,
            Name = registerDto.Name
        };

        try
        {
            var result = await userManager.CreateAsync(appUser, registerDto.Password);
            if(result.Succeeded) 
            {
                var user = appDbContext.AppUsers.First(u=> u.UserName == registerDto.Email);
                UserDto userDto = new()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber
                };
                return "";
            }
            else
            {
                return result.Errors.FirstOrDefault().Description;
            }
        }
        catch (Exception ex)
        {

        }
        return "Error encountered"; 
    }
}
