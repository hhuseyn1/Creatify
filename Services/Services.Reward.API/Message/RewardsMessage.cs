namespace Services.Reward.API.Message;

public class RewardsMessage
{
    public Guid UserId { get; set; }
    public int RewardActivity { get; set; }
    public Guid OrderId { get; set; }
}
