using Microsoft.AspNetCore.Identity;
using Services.Auth.API.Data;
using Services.Auth.API.Models;
using Services.Auth.API.Models.Dto;
using Services.Auth.API.Services.IAuth;
using Services.Auth.API.Services.IService;

namespace Services.Auth.API.Services;

public class AuthService : IAuthService
{
	private readonly AppDbContext appDbContext;
	private readonly UserManager<AppUser> userManager;
	private readonly RoleManager<IdentityRole> roleManager;
	private readonly IJwtTokenGenerator jwtGenerator;

	public AuthService(AppDbContext appDbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtGenerator)
	{
		this.appDbContext = appDbContext;
		this.userManager = userManager;
		this.roleManager = roleManager;
		this.jwtGenerator = jwtGenerator;
	}

	public async Task<bool> AssignRole(string email, string roleName)
	{
		var user = appDbContext.AppUsers.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
		if (user is not null)
		{
			if (!roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
			{
				roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
			}
			await userManager.AddToRoleAsync(user, roleName);
			return true;
		}
		return false;
	}

	public async Task<Creatify.Web.Models.LoginResponseDto> Login(LoginDto loginDto)
	{
		var user = appDbContext.Users.FirstOrDefault(u => u.UserName.ToLower() == loginDto.UserName.ToLower());

		bool isValid = await userManager.CheckPasswordAsync(user, loginDto.Password);

		if (user is null || isValid is false)
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
			UserId = Guid.Parse(user.Id)
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
				var user = appDbContext.AppUsers.First(u => u.UserName == registerDto.Email);
				Creatify.Web.Models.UserDto userDto = new()
				{
					UserId = Guid.Parse(user.Id),
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
