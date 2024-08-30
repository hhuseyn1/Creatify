using Services.Reward.API.Message;

namespace Services.Reward.API.Services;

public interface IRewardService
{
    Task UpdateRewards(RewardsMessage rewardsMessage);
}
