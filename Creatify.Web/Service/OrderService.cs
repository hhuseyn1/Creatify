using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Creatify.Web.Utility;

namespace Creatify.Web.Service;

public class OrderService : IOrderService
{
    private readonly IBaseService _baseService;

    public OrderService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto> CreateOrder(CartDto cartDto)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.POST,
            Data = cartDto,
            Url = StaticDetails.CouponAPIBase + "/api/order/CreateOrder"
        });
    }

    public async Task<ResponseDto?> CreateStripeSession(StripeRequestDto requestDto)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.POST,
            Data = requestDto,
            Url = StaticDetails.CouponAPIBase + "/api/order/CreateStripeSession"
        });
    }

    public async Task<ResponseDto?>? GetAllOrdersbyUserId(string userId)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.GET,
            Url = StaticDetails.CouponAPIBase + "/api/order/GetAllOrdersbyUserId/" + userId
        });
    }

    public async Task<ResponseDto?>? GetOrdersbyId(string id)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.GET,
            Url = StaticDetails.CouponAPIBase + "/api/order/GetOrdersbyId/" + id
        });
    }

    public async Task<ResponseDto?> UpdateOrderStatus(string orderId, string newStatus)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.PUT,
            Data= newStatus,
            Url = StaticDetails.CouponAPIBase + "/api/order/UpdateOrderStatus/" + orderId
        });
    }

    public async Task<ResponseDto?> ValidateStripeSession(string orderHeaderId)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.POST,
            Data = orderHeaderId,
            Url = StaticDetails.CouponAPIBase + "/api/order/ValidateStripeSession" + orderHeaderId
        });
    }
}
