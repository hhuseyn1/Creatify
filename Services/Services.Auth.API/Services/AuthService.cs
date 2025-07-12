using Creatify.Web.Utility;
using Microsoft.AspNetCore.Identity;
using Services.Auth.API.Data;
using Services.Auth.API.Models;
using Services.Auth.API.Models.Dto;
using Services.Auth.API.Services.IAuth;
using Services.Auth.API.Services.IService;

namespace Services.Auth.API.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtGenerator;

    public AuthService(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtGenerator)
    {
        this._context = context;
        this._userManager = userManager;
        this._roleManager = roleManager;
        this._jwtGenerator = jwtGenerator;
    }

    public async Task<bool> AssignRole(string email, string roleName)
    {
        var user = _context.AppUsers.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
        if (user is not null)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }
        return false;
    }

    public async Task<LoginResponseDto> Login(LoginDto loginDto)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName.ToLower() == loginDto.Email.ToLower());

        bool isValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (user is null || !isValid)
        {
            return new LoginResponseDto() { Token = "", User = null };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtGenerator.GenerateToken(user, roles);

        UserDto userDto = new()
        {
            Email = user.Email,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber,
            Id = Guid.Parse(user.Id)
        };

        LoginResponseDto loginResponseDto = new()
        {
            Token = token,
            User = userDto
        };

        return loginResponseDto;
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
            var result = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(StaticDetails.RoleCustomer))
                    await _roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleCustomer));

                await _userManager.AddToRoleAsync(appUser, StaticDetails.RoleCustomer);

                return "";
            }
            else
            {
                return result.Errors.FirstOrDefault().Description;
            }
        }
        catch (Exception)
        {
            return "Error encountered";
        }
    }
}
