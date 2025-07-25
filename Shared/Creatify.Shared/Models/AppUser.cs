using Microsoft.AspNetCore.Identity;

namespace Creatify.Shared.Models;

public class AppUser : IdentityUser
{
	public string Name { get; set; }
    public ICollection<Address>? Addresses { get; set; }
}
