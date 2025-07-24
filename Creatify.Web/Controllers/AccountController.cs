using Microsoft.AspNetCore.Mvc;

namespace Creatify.Web.Controllers;

public class AccountController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
