using Newtonsoft.Json;
using Services.ShoppingCart.API.Models.Dto;
using Services.ShoppingCart.API.Service.IService;

namespace Services.ShoppingCart.API.Service;

public class CouponService : ICouponService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CouponService(IHttpClientFactory httpClientFactory)
    {
        this._httpClientFactory = httpClientFactory;
    }

    public async Task<CouponDto> GetCouponAsync(string couponCode)
    {
        var client = _httpClientFactory.CreateClient("Coupon");
        var response = await client.GetAsync($"/api/coupon/GetCouponbyCode/{couponCode}");
        var apiContent = await response.Content.ReadAsStringAsync();

        var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        if (resp!=null && resp.isSuccess)
        {
            return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
        }

        return new CouponDto();
    }
}
