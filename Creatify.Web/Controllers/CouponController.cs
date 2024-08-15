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
		else
			TempData["error"] = responseDto?.Message;
		return View(list);
	}

	public IActionResult CouponCreate()
	{
		return View();
	}


	[HttpPost]
	public async Task<IActionResult> CouponCreate(CouponDto couponDto)
	{
		if (ModelState.IsValid)
		{
			ResponseDto? responseDto = await service.CreateCouponAsync(couponDto);
			if (responseDto.isSuccess && responseDto != null)
			{
				TempData["success"] = "Coupon created successfully!";
				return RedirectToAction(nameof(CouponIndex));
			}
			else
				TempData["error"] = responseDto?.Message;
		}
		return View(couponDto);
	}

	public async Task<IActionResult> CouponDelete(Guid id)
	{
		ResponseDto? responseDto = await service.DeleteCouponAsync(id);
		if (responseDto.isSuccess && responseDto != null)
		{
			TempData["success"] = "Coupon deleted successfully!";
			return RedirectToAction(nameof(CouponIndex));
		}
		else
			TempData["error"] = responseDto?.Message;
		return NotFound();
	}


}
