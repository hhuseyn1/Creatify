using Services.Product.API.Models.Dto;

namespace Services.Product.API.Service.IService;

public interface IProductService
{
    ResponseDto GetAllProducts();
    ResponseDto GetProductById(Guid id);
    ResponseDto AddProduct(ProductDto productDto, HttpContext httpContext);
    ResponseDto EditProduct(ProductDto productDto, HttpContext httpContext);
    ResponseDto DeleteProductById(Guid id);
}
