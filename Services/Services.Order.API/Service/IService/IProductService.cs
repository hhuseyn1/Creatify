using Services.Order.API.Models.Dto;

namespace Services.Order.API.Service.IService;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync();
}
