using Creatify.Web.Models;
using Creatify.Web.Models.Dto;
using Creatify.Web.Service.IService;
using Creatify.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Creatify.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;

    public CartController(ICartService cartService, IOrderService orderService)
    {
        this._cartService = cartService;
        this._orderService = orderService;
    }

    [Authorize]
    public async Task<IActionResult> CartIndex()
    {
        return View(await LoadCartDtoBasedOnLoggedInUser());
    }

    [Authorize]
    public async Task<IActionResult> Checkout()
    {
        return View(await LoadCartDtoBasedOnLoggedInUser());
    }

    [HttpPost]
    [ActionName("Checkout")]
    public async Task<IActionResult> Checkout(CartDto cartDto)
    {
        CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
        cart.CartHeader.Fullname = cartDto.CartHeader.Fullname;
        cart.CartHeader.Email = cartDto.CartHeader.Email;
        cart.CartHeader.Phone = cartDto.CartHeader.Phone;

        var response = await _orderService.CreateOrder(cart);
        OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

        if (response != null && response.isSuccess)
        {
            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            StripeRequestDto requestDto = new()
            {
                ApprovedUrl = domain + "cart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
                CancelUrl = domain + "cart/checkout",
                OrderHeader = orderHeaderDto
            };
            var stripeResponse = await _orderService.CreateStripeSession(requestDto);
            StripeRequestDto stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDto>(Convert.ToString(stripeResponse.Result));
            Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
            return new StatusCodeResult(303);
        }
        return View();
    }

    public async Task<IActionResult> Confirmation(string orderId)
    {
        ResponseDto response = await _orderService.ValidateStripeSession(orderId);

        if (response != null && response.isSuccess)
        {
            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
            if (orderHeaderDto.Status == StaticDetails.Status_Approved)
            {
                return View(orderId);
            }
        }
        return View(orderId);
    }
    public async Task<IActionResult> Remove(string cartDetailsId)
    {
        var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        ResponseDto response = await _cartService.RemoveFromCartAsync(cartDetailsId);

        if (response != null && response.isSuccess)
        {
            TempData["success"] = "Cart updated successfully";
        }
        else
        {
            TempData["error"] = "Cart removal failed";
        }
        return RedirectToAction(nameof(CartIndex));
    }

    [HttpPost]
    public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
    {
        ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);

        if (response != null & response.isSuccess)
        {
            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> EmailCart(CartDto cartDto)
    {
        CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
        cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
        ResponseDto response = await _cartService.EmailCart(cartDto);

        if (response != null && response.isSuccess)
        {
            TempData["success"] = "Email will be processed and send shortly.";
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
    {
        cartDto.CartHeader.CouponCode = "";
        ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);

        if (response != null && response.isSuccess)
        {
            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }

    private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
    {
        var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        ResponseDto response = await _cartService.GetCartbyUserIdAsync(userId);

        if (response != null && response.isSuccess)
        {
            CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            return cartDto;
        }
        return new CartDto();
    }
}
