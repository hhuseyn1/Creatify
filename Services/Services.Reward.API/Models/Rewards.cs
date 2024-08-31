namespace Services.Reward.API.Models;

public class Rewards
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime RewardsDate { get; set; }
    public int RewardsActivity { get; set; }
    public Guid OrderId { get; set; }
}
