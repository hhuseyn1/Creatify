using Creatify.Web.Models;
using Creatify.Web.Service.IService;

namespace Creatify.Web.Service;

public class CouponService : ICouponService
{
    private readonly IBaseService _baseService;

    public CouponService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public Task<ResponseDto> CreateCouponAsync(CouponDto coupon)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> DeleteCouponAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> GetAllCouponsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> GetCouponAsync(string CouponCode)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> GetCouponByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> UpdateCouponAsync(CouponDto coupon)
    {
        throw new NotImplementedException();
    }
}
