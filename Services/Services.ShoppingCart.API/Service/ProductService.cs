using Newtonsoft.Json;
using Services.ShoppingCart.API.Models.Dto;
using Services.ShoppingCart.API.Service.IService;

namespace Services.ShoppingCart.API.Service;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        var client = _httpClientFactory.CreateClient("Product");
        var response = await client.GetAsync($"/api/product/GetAllProducts");
        var apiContent = await response.Content.ReadAsStringAsync();

        var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        if (resp.isSuccess)
        {
            return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
        }

        return new List<ProductDto>();
    }
}
