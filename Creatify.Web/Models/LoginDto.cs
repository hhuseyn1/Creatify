﻿using System.ComponentModel.DataAnnotations;

namespace Services.Auth.API.Models.Dto;

public class LoginDto
{
	[Required]
	public string UserName { get; set; }
	[Required]
	public string Password { get; set; }
}
