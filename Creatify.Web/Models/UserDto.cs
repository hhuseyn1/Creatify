namespace Creatify.Web.Models;

public class UserDto
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
    public Gender? Gender { get; set; }           
    public DateTime? DateOfBirth { get; set; }    
    public int? Height { get; set; } // Height in centimeters       
    public int? Weight { get; set; } // Weight in kilograms
}

public enum Gender
{
    Male,
    Female
}