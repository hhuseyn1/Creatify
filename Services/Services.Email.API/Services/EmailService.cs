using Microsoft.EntityFrameworkCore;
using Services.Email.API.Data;
using Services.Email.API.Message;
using Services.Email.API.Models;
using Services.Email.Models.Dto;
using System.Text;

namespace Services.Email.API.Services;

public class EmailService : IEmailService
{
    private DbContextOptions<AppDbContext> _dbOptions;

    public EmailService(DbContextOptions<AppDbContext> dbOptions)
    {
        this._dbOptions = dbOptions;
    }

    public async Task EmailCartAndLog(CartDto cartDto)
    {
        StringBuilder message = new StringBuilder();

        message.AppendLine("<br/>Cart Email requested");
        message.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
        message.AppendLine("<br/>");
        message.AppendLine("<ul>");

        foreach (var item in cartDto.CartDetails)
        {
            message.AppendLine("<li>");
            message.AppendLine(item.Product.Name + " x " + item.Count);
            message.AppendLine("</li>");
        }
        message.AppendLine("</ul>");

        await LogAndEmail(message.ToString(),cartDto.CartHeader.Email);
    }

    public async Task LogOrderPlaced(RewardsMessage rewardsMessage)
    {
        string message = "New Order Placed. <br/>Order ID: " + rewardsMessage.OrderId;
        await LogAndEmail(message,"huseyn.hemidov.2004@gmail.com");
    }

    public async Task RegisterUserEmailAndLog(string email)
    {
        string message = "User Registration successfull. <br/> Email : " + email;
        await LogAndEmail(message, "huseyn.hemidov.2004@gmail.com");
    }

    private async Task<bool> LogAndEmail(string message, string email)
    {
        try
        {
            EmailLogger emailLogger = new()
            {
                Email = email,
                EmailSent = DateTime.Now,
                Message = message
            };

            await using var _db = new AppDbContext(_dbOptions);
            await _db.EmailLoggers.AddAsync(emailLogger);
            await _db.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
