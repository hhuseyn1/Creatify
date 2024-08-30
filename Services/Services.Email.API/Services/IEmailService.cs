using Services.Email.API.Message;
using Services.Email.Models.Dto;
namespace Services.Email.API.Services;

public interface IEmailService
{
    Task EmailCartAndLog(CartDto cartDto);
    Task RegisterUserEmailAndLog(string email);
    Task LogOrderPlaced(RewardsMessage rewardsMessage);
}
