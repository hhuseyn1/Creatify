using Creatify.Web.Models;
using Creatify.Web.Models.Dto;
using Creatify.Web.Service.IService;
using Creatify.Web.Utility;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    public IActionResult OrderIndex()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> GetOrderDetailbyId(Guid orderId)
    {
        OrderHeaderDto orderHeader = new OrderHeaderDto();
        string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault().Value;
        var response = await _orderService.GetOrdersbyId(orderId.ToString());

        if (response != null && response.isSuccess)
        {
            orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
        }
        if (!User.IsInRole(StaticDetails.RoleAdmin) && userId != orderHeader.UserId.ToString())
        {
            return NotFound();
        }
        return View(orderHeader);
    }


    [HttpPost("OrderReadyForPickup")]
    public async Task<IActionResult> OrderReadyForPickup(Guid orderId)
    {
        var response = await _orderService.UpdateOrderStatus(orderId.ToString(), StaticDetails.Status_ReadyForPickup);
        if (response != null && response.isSuccess)
        {
            TempData["success"] = "Status updated successfully";
            return RedirectToAction(nameof(GetOrderDetailbyId), new { orderId = orderId });
        }
        return View();
    }

    [HttpPost("CompleteOrder")]
    public async Task<IActionResult> CompleteOrder(Guid orderId)
    {
        var response = await _orderService.UpdateOrderStatus(orderId.ToString(), StaticDetails.Status_Completed);
        if (response != null && response.isSuccess)
        {
            TempData["success"] = "Status updated successfully";
            return RedirectToAction(nameof(GetOrderDetailbyId), new { orderId = orderId });
        }
        return View();
    }

    [HttpPost("CancelOrder")]
    public async Task<IActionResult> CancelOrder(Guid orderId)
    {
        var response = await _orderService.UpdateOrderStatus(orderId.ToString(), StaticDetails.Status_Cancelled);
        if (response != null && response.isSuccess)
        {
            TempData["success"] = "Status updated successfully";
            return RedirectToAction(nameof(GetOrderDetailbyId), new { orderId = orderId });
        }
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> GetAllOrders(string status)
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
            switch (status)
            {
                case "approved":
                    headers = headers.Where(u => u.Status == StaticDetails.Status_Approved);
                    break;
                case "cancelled":
                    headers = headers.Where(u => u.Status == StaticDetails.Status_Cancelled || u.Status == StaticDetails.Status_Refunded);
                    break;
                case "readyforpickup":
                    headers = headers.Where(u => u.Status == StaticDetails.Status_ReadyForPickup);
                    break;
                default:
                    break;
            }
        }
        else
        {
            headers = new List<OrderHeaderDto>();
        }

        return Json(new { data = headers });
    }

}
