using Creatify.Web.Models;

namespace Creatify.Web.Service.IService;

public interface IProductService
{
	Task<ResponseDto> GetAllProductsAsync();
	Task<ResponseDto> GetProductByIdAsync(Guid id);
	Task<ResponseDto> CreateProductAsync(ProductDto productDto);
	Task<ResponseDto> UpdateProductAsync(ProductDto productDto);
	Task<ResponseDto> DeleteProductAsync(Guid id);
}
