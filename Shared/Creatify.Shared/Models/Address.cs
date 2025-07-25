namespace Creatify.Shared.Models;

public class Address
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public string Line1 { get; set; } = null!;
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
