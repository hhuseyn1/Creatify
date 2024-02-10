using Microsoft.AspNetCore.Identity;

namespace Services.Auth.API.Models;

public class AppUser : IdentityUser
{
    public string Name { get; set; }
}
