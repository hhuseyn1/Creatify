using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Coupon.API.Data;

namespace Services.Coupon.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CouponController : ControllerBase
{
    private readonly AppDbContext _context;

    public CouponController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public object GetCoupon()
    {
        try
        {
            IEnumerable<Models.Coupon> objList = _context.Coupons.ToList();
            return objList;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

    [HttpGet]
    [Route("{id:int}")]
    public object GetCouponbyId(int id)
    {
        try
        {
            Models.Coupon obj = _context.Coupons.First(u=>u.Id == id);
            return obj;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

}
