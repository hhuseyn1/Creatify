namespace Services.Reward.API.Message;

public class RewardsMessage
{
    public string UserId { get; set; }
    public int RewardActivity { get; set; }
    public string OrderId { get; set; }
}
