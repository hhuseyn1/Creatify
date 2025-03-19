using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Coupon.API.Data;
using Services.Coupon.API.Models.Dto;

namespace Services.Coupon.API.Controllers;

[Route("api/coupon")]
[ApiController]
public class CouponAPIController : ControllerBase 
{
	private readonly AppDbContext _context;
	private ResponseDto _response;
	public IMapper _mapper;

	public CouponAPIController(AppDbContext context, IMapper mapper)
	{
		_context = context;
		_response = new ResponseDto();
		_mapper = mapper;
	}

	[HttpGet]
	[Authorize(Roles = "ADMIN")]
    [Route("GetAllCoupons")]
	public ResponseDto GetAllCoupons()
	{
		try
		{
			IEnumerable<Models.Coupon> objList = _context.Coupons.ToList();
			_response.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);
		}
		catch (Exception ex)
		{
			_response.isSuccess = false;
			_response.Message = ex.Message;
		}
		return _response;
	}

	[HttpGet]
	[Authorize]
	[Route("{id:guid}")]
	public ResponseDto GetCouponbyId(string id)
	{
		try
		{
			Models.Coupon obj = _context.Coupons.First(u => u.Id.ToString() == id);
			_response.Result = _mapper.Map<CouponDto>(obj);
		}
		catch (Exception ex)
		{
			_response.isSuccess = false;
			_response.Message = ex.Message;
		}
		return _response;
	}

	[HttpGet]
	[Authorize]
    [Route("GetCouponbyCode/{code}")]
	public ResponseDto GetCouponbyCode(string code)
	{
		try
		{
			Models.Coupon obj = _context.Coupons.First(u => u.CouponCode.ToLower() == code.ToLower());
			_response.Result = _mapper.Map<CouponDto>(obj);
		}
		catch (Exception ex)
		{
			_response.isSuccess = false;
			_response.Message = ex.Message;
		}
		return _response;
	}

	[HttpPost]
	[Authorize(Roles = "ADMIN")]
	[Route("AddCoupon")]
	public ResponseDto AddCoupon([FromBody] CouponDto couponDto)
	{
		try
		{
			Models.Coupon obj = _mapper.Map<Models.Coupon>(couponDto);
			_context.Add(obj);
			_context.SaveChanges();

			var options = new Stripe.CouponCreateOptions
			{
				AmountOff = (long)(couponDto.DiscountAmount*100),
				Name = couponDto.CouponCode,
				Currency = "usd",
				Id = couponDto.CouponCode
			};
			var service = new Stripe.CouponService();
			service.Create(options);

			_response.Result = _mapper.Map<CouponDto>(obj);
		}
		catch (Exception ex)
		{
			_response.isSuccess = false;
			_response.Message = ex.Message;
		}
		return _response;
	}

	[HttpPut]
	[Authorize(Roles = "ADMIN")]
	[Route("EditCoupon")]
	public ResponseDto EditCoupon([FromBody] CouponDto couponDto)
	{
		try
		{
			Models.Coupon obj = _mapper.Map<Models.Coupon>(couponDto);
			_context.Update(obj);
			_context.SaveChanges();

			_response.Result = _mapper.Map<CouponDto>(obj);
		}
		catch (Exception ex)
		{
			_response.isSuccess = false;
			_response.Message = ex.Message;
		}
		return _response;
	}

	[HttpDelete]
	[Authorize(Roles = "ADMIN")]
	[Route("DeleteCouponbyId/{id}")]
	public ResponseDto DeleteCouponbyId(string id)
	{
		try
		{
			Models.Coupon obj = _context.Coupons.First(u => u.Id.ToString() == id);
			_context.Remove(obj);
			_context.SaveChanges();

            var service = new Stripe.CouponService();
            service.Delete(obj.CouponCode);


            _response.Result = _mapper.Map<CouponDto>(obj);
		}
		catch (Exception ex)
		{
			_response.isSuccess = false;
			_response.Message = ex.Message;
		}
		return _response;
	}
}
