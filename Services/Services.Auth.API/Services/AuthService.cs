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
	private readonly UserManager<AppUser> userManager;
	private readonly RoleManager<IdentityRole> roleManager;
	private readonly IJwtTokenGenerator jwtGenerator;

	public AuthService(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtGenerator)
	{
		this._context = context;
		this.userManager = userManager;
		this.roleManager = roleManager;
		this.jwtGenerator = jwtGenerator;
	}

	public async Task<bool> AssignRole(string email, string roleName)
	{
		var user = _context.AppUsers.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
		if (user is not null)
		{
			if (!await roleManager.RoleExistsAsync(roleName))
			{
				await roleManager.CreateAsync(new IdentityRole(roleName));
			}
			await userManager.AddToRoleAsync(user, roleName);
			return true;
		}
		return false;
	}

	public async Task<Creatify.Web.Models.LoginResponseDto> Login(LoginDto loginDto)
	{
		var user = _context.Users.FirstOrDefault(u => u.UserName.ToLower() == loginDto.UserName.ToLower());

		bool isValid = await userManager.CheckPasswordAsync(user, loginDto.Password);

		if (user is null || !isValid)
		{
			return new Creatify.Web.Models.LoginResponseDto() { Token = "", User = null };
		}

		var roles = await userManager.GetRolesAsync(user);
		var token = jwtGenerator.GenerateToken(user, roles);

		Creatify.Web.Models.UserDto userDto = new()
		{
			Email = user.Email,
			Name = user.Name,
			PhoneNumber = user.PhoneNumber,
			Id = Guid.Parse(user.Id)
		};

		Creatify.Web.Models.LoginResponseDto loginResponseDto = new()
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
            var result = await userManager.CreateAsync(appUser, registerDto.Password);
            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(StaticDetails.RoleCustomer))
                    await roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleCustomer));

                await userManager.AddToRoleAsync(appUser, StaticDetails.RoleCustomer);

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
