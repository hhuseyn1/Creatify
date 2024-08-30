using Creatify.Web.Models;

namespace Creatify.Web.Service.IService;

public interface ICartService
{
    Task<ResponseDto?> GetCartbyUserIdAsync(string userId);
    Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
    Task<ResponseDto?> RemoveFromCartAsync(string cartDetailsId);
    Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);
    Task<ResponseDto?> EmailCart(CartDto cartDto);
}
