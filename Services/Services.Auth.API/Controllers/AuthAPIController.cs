﻿using Microsoft.AspNetCore.Mvc;
using Services.Auth.API.Models.Dto;
using Services.Auth.API.Services.IAuth;
using System.Diagnostics.Contracts;

namespace Services.Auth.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _responseDto;
        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var login = await _authService.Login(loginDto);
            if (string.IsNullOrEmpty(login.ToString()))
            {
                _responseDto.isSuccess = false;
                _responseDto.Message = "Username or password is incorrect!";
                return BadRequest(_responseDto);
            }
            _responseDto.Result = login;
            return Ok(_responseDto);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var message = await _authService.Register(registerDto);
            if (!string.IsNullOrEmpty(message))
            {
                _responseDto.isSuccess = false;
                _responseDto.Message = message;
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterDto registerDto)
        {
            var assignRole = await _authService.AssignRole(registerDto.Email,registerDto.Role.ToUpper());
            if (!assignRole)
            {
                _responseDto.isSuccess = false;
                _responseDto.Message = "Error occured";
                return BadRequest(_responseDto);
            }
            _responseDto.Result = $"{registerDto.Role} are successfully given to {registerDto.Email}"; 
            return Ok(_responseDto);
        }
    }
}
