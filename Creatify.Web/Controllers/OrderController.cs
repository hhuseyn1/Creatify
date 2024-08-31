using Creatify.Web.Models;
using Creatify.Web.Models.Dto;
using Creatify.Web.Service.IService;
using Creatify.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;

namespace Creatify.Web.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        this._orderService = orderService;
    }

    public IActionResult OrderIndex()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        IEnumerable<OrderHeaderDto> headers;
        string userId = "";

        if (User.IsInRole(StaticDetails.RoleAdmin))
        {
            userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault().Value;
        }
        ResponseDto responseDto = await _orderService.GetAllOrdersbyUserId(userId);
        if (responseDto != null && responseDto.isSuccess)
        {
            headers = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(responseDto.Result));
        }
        else
        {
            headers = new List<OrderHeaderDto>();
        }

        return Json(new { data = headers });
    }


}
