namespace Services.Reward.API.Models;

public class Rewards
{
    public string Id { get; set; }
    public string userId { get; set; }
    public DateTime RewardsDate { get; set; }
    public int RewardsActivity { get; set; }
    public string orderId { get; set; }
}
