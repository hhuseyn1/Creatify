namespace Services.User.API.Models.Dto;

public class AddressDto
{
    public Guid Id { get; set; }
    public string Line1 { get; set; } = null!;
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
}
