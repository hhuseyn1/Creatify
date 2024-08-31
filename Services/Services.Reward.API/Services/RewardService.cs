using Microsoft.EntityFrameworkCore;
using Services.Reward.API.Data;
using Services.Reward.API.Message;
using Services.Reward.API.Models;

namespace Services.Reward.API.Services;

public class RewardService : IRewardService
{
    private DbContextOptions<AppDbContext> _dbOptions;

    public RewardService(DbContextOptions<AppDbContext> dbOptions)
    {
        this._dbOptions = dbOptions;
    }

    public async Task UpdateRewards(RewardsMessage rewardsMessage)
    {
        try
        {
            Rewards rewards = new()
            {
                OrderId = rewardsMessage.OrderId,
                RewardsActivity = rewardsMessage.RewardActivity,
                UserId = rewardsMessage.UserId,
                RewardsDate = DateTime.Now
            };
            await using var _db = new AppDbContext(_dbOptions);
            await _db.Rewards.AddAsync(rewards);
            await _db.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
