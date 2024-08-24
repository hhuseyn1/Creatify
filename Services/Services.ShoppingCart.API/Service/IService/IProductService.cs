using Services.ShoppingCart.API.Models.Dto;

namespace Services.ShoppingCart.API.Service.IService;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync();
}
