using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Creatify.Web.Controllers;

public class CouponController : Controller
{
    private readonly ICouponService service;

    public CouponController(ICouponService service)
    {
        this.service = service;
    }
    public async Task<IActionResult> CouponIndex()
    {
        List<CouponDto> list = new();
        ResponseDto responseDto = await service.GetAllCouponsAsync();
        if (responseDto.isSuccess && responseDto != null)
            list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(responseDto.Result));
        return View(list);
    }

}
