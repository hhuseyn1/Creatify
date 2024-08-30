using Creatify.Web.Models;
namespace Creatify.Web.Service.IService;

public interface IOrderService
{
    Task<ResponseDto?> CreateOrder(CartDto cartDto);
    Task<ResponseDto?> CreateStripeSession(StripeRequestDto requestDto);
    Task<ResponseDto?> ValidateStripeSession(string orderHeaderId);
}   
