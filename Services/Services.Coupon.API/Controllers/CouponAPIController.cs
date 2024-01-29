using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Services.Coupon.API.Data;
using Services.Coupon.API.Models.Dto;

namespace Services.Coupon.API.Controllers;

[Route("api/[controller]")]
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
    [Route("{id:int}")]
    public ResponseDto GetCouponbyId(int id)
    {
        try
        {
            Models.Coupon obj = _context.Coupons.First(u=>u.Id == id);
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
    [Route("AddCoupon")]
    public ResponseDto AddCoupon([FromBody] CouponDto couponDto)
    {
        try
        {
            Models.Coupon obj = _mapper.Map<Models.Coupon>(couponDto);
            _context.Add(obj);
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

    [HttpPut]
    [Route("EditCouponbyId/{id}")]
    public ResponseDto EditCouponbyId([FromBody] CouponDto couponDto)
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
    [Route("DeleteCouponbyId/{id}")]
    public ResponseDto DeleteCouponbyId(int id)
    {
        try
        {
            Models.Coupon obj = _context.Coupons.First(u => u.Id == id);
            _context.Remove(obj);
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

}
