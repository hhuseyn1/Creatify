namespace Services.Email.API.Models;

public class EmailLogger
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public DateTime? EmailSent { get; set; }
}
