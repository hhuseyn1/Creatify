﻿namespace Services.Auth.API.Models.Dto;

public class RegisterDto
{
	public string Name { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public string Password { get; set; }
	public string? Role { get; set; }
}
