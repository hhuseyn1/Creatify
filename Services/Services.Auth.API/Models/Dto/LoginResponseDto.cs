namespace Services.Auth.API.Models.Dto;

public class LoginResponseDto
{
    public UserDto User { get; set; }
    public string Token { get; set; }
}
