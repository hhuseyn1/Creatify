using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Creatify.Web.Utility;

namespace Creatify.Web.Service;

public class CouponService : ICouponService
{
	private readonly IBaseService _baseService;

	public CouponService(IBaseService baseService)
	{
		_baseService = baseService;
	}

	public async Task<ResponseDto> CreateCouponAsync(CouponDto couponDto)
	{
		return await _baseService.SendAsync(new()
		{
			APIType = StaticDetails.APIType.POST,
			Data = couponDto,
			Url = StaticDetails.CouponAPIBase + "/api/coupon/AddCoupon"
		});
	}

	public async Task<ResponseDto> DeleteCouponAsync(Guid id)
	{
		return await _baseService.SendAsync(new()
		{
			APIType = StaticDetails.APIType.DELETE,
			Url = StaticDetails.CouponAPIBase + "/api/coupon/DeleteCouponbyId/" + id
		});
	}

	public async Task<ResponseDto> GetAllCouponsAsync()
	{
		return await _baseService.SendAsync(new()
		{
			APIType = StaticDetails.APIType.GET,
			Url = StaticDetails.CouponAPIBase + "/api/coupon/GetAllCoupons"
		});
	}

	public async Task<ResponseDto> GetCouponAsync(string CouponCode)
	{
		return await _baseService.SendAsync(new()
		{
			APIType = StaticDetails.APIType.GET,
			Url = StaticDetails.CouponAPIBase + "/api/coupon/GetCouponbyCode/" + CouponCode
		});
	}

	public async Task<ResponseDto> GetCouponByIdAsync(Guid id)
	{
		return await _baseService.SendAsync(new()
		{
			APIType = StaticDetails.APIType.GET,
			Url = StaticDetails.CouponAPIBase + "/api/coupon/" + id
		});
	}

	public async Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto)
	{
		return await _baseService.SendAsync(new()
		{
			APIType = StaticDetails.APIType.PUT,
			Data = couponDto,
			Url = StaticDetails.CouponAPIBase + "/api/coupon/EditCoupon"
		});
	}
}
