using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Coupon.API.Data;
using Services.Coupon.API.Models.Dto;

namespace Services.Coupon.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CouponAPIController : ControllerBase
{
    private readonly AppDbContext _context;
    private ResponseDto _response;

    public CouponAPIController(AppDbContext context)
    {
        _context = context;
        _response = new ResponseDto();
    }

    [HttpGet]
    public ResponseDto GetCoupon()
    {
        try
        {
            IEnumerable<Models.Coupon> objList = _context.Coupons.ToList();
            _response.Result = objList;
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
            _response.Result = obj;
        }
        catch (Exception ex)
        {
            _response.isSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

}
