namespace Services.Order.API.Models.Dto;

public class RewardsDto
{
    public Guid UserId { get; set; }
    public int RewardsActivity { get; set; }
    public Guid OrderId { get; set; }
}
