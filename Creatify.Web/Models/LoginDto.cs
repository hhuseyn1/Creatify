using System.ComponentModel.DataAnnotations;

namespace Creatify.Web.Models;

public class LoginDto
{
	public string UserName { get; set; }
	public string Password { get; set; }
}
