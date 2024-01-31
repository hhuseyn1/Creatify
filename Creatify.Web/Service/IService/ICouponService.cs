using Creatify.Web.Models;

namespace Creatify.Web.Service.IService;

public interface ICouponService
{
    Task<ResponseDto> GetCouponAsync(string CouponCode);
    Task<ResponseDto> GetAllCouponsAsync();
    Task<ResponseDto> GetCouponByIdAsync(int id);
    Task<ResponseDto> CreateCouponAsync(CouponDto coupon);
    Task<ResponseDto> UpdateCouponAsync(CouponDto coupon);
    Task<ResponseDto> DeleteCouponAsync(int id);
}
