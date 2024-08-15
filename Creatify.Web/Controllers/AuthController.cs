using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Creatify.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Services.Auth.API.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Creatify.Web.Controllers;

public class AuthController : Controller
{
	private readonly IAuthService _authService;
	private readonly ITokenProvider _tokenProvider;

	public AuthController(IAuthService authService, ITokenProvider tokenProvider)
	{
		_authService = authService;
		_tokenProvider = tokenProvider;
	}

	[HttpGet]
	public IActionResult Login()
	{
		LoginDto loginDto = new();
		return View(loginDto);
	}

	[HttpPost]
	public async Task<IActionResult> Login(LoginDto loginDto)
	{
		ResponseDto responseDto = await _authService.LoginAsync(loginDto);
		if (responseDto != null && responseDto.isSuccess)
		{
			LoginResponseDto loginResponseDto = JsonConvert
				.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

			await SignInAsync(loginResponseDto);
			_tokenProvider.SetToken(loginResponseDto.Token);

			return RedirectToAction("Index", "Home");
		}
		else
		{
			TempData["Error"] = responseDto.Message;
			return View(loginDto);
		}
	}

	[HttpGet]
	public IActionResult Register()
	{
		var roleList = new List<SelectListItem>()
		{
			new SelectListItem{Text = StaticDetails.RoleAdmin, Value = StaticDetails.RoleAdmin },
			new SelectListItem{Text = StaticDetails.RoleCustomer, Value = StaticDetails.RoleCustomer }
		};

		ViewBag.RoleList = roleList;
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Register(RegisterDto registerDto)
	{
		ResponseDto responseDto = await _authService.RegisterAsync(registerDto);
		ResponseDto assignRole;

		if (responseDto != null && responseDto.isSuccess)
		{
			if (string.IsNullOrEmpty(registerDto.Role))
				registerDto.Role = StaticDetails.RoleCustomer;

			assignRole = await _authService.AssignRoleAsync(registerDto);
			if (assignRole != null && assignRole.isSuccess)
			{
				TempData["Success"] = "Registration Successfully";
				return RedirectToAction(nameof(Login));

			}
		}
		else
		{
			TempData["Error"] = responseDto.Message;
		}

		var roleList = new List<SelectListItem>()
		{
			new SelectListItem{Text = StaticDetails.RoleAdmin, Value = StaticDetails.RoleAdmin },
			new SelectListItem{Text = StaticDetails.RoleCustomer, Value = StaticDetails.RoleCustomer }
		};

		ViewBag.RoleList = roleList;
		return View(registerDto);
	}

	[HttpGet]
	public async Task<IActionResult> Logout()
	{
		await HttpContext.SignOutAsync();
		_tokenProvider.ClearToken();
		return RedirectToAction("Index", "Home");
	}

	private async Task SignInAsync(LoginResponseDto loginDto)
	{
		var handler = new JwtSecurityTokenHandler();

		var jwt = handler.ReadJwtToken(loginDto.Token);

		var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

		identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
			jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
		identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
			jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
		identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
					jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

		identity.AddClaim(new Claim(ClaimTypes.Name,
			jwt.Claims.FirstOrDefault(U => U.Type == JwtRegisteredClaimNames.Email).Value));

		identity.AddClaim(new Claim(ClaimTypes.Role,
			jwt.Claims.FirstOrDefault(U => U.Type == "role").Value));

		var principal = new ClaimsPrincipal(identity);
		await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
	}
}
