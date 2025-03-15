using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Creatify.Web.Utility;

namespace Creatify.Web.Service;

public class CartService : ICartService
{
    private readonly IBaseService _baseService;

    public CartService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.POST,
            Data = cartDto,
            Url = StaticDetails.CouponAPIBase + "/api/cart/ApplyCoupon"
        });
    }

    public async Task<ResponseDto?> EmailCart(CartDto cartDto)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.POST,
            Data = cartDto,
            Url = StaticDetails.CouponAPIBase + "/api/cart/EmailCartRequest"
        });
    }

    public async Task<ResponseDto?> GetCartbyUserIdAsync(string userId)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.GET,
            Url = StaticDetails.CouponAPIBase + "/api/cart/GetCartbyUserId/" + userId
        });
    }

    public async Task<ResponseDto?> RemoveFromCartAsync(string cartDetailsId)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.POST,
            Data = cartDetailsId,
            Url = StaticDetails.CouponAPIBase + "/api/cart/RemoveCartbyId/" + cartDetailsId
        });
    }

    public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.POST,
            Data = cartDto,
            Url = StaticDetails.CouponAPIBase + "/api/cart/UpsertCart"
        });
    }
}
