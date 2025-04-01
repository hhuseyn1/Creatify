using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Product.API.Models.Dto;
using Services.Product.API.Service.IService;

[Route("api/product")]
[ApiController]
public class ProductAPIController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductAPIController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("GetAllProducts")]
    public ResponseDto GetAllProducts() => _productService.GetAllProducts();

    [HttpGet("GetProductById/{id}")]
    public ResponseDto GetProductById(Guid id) => _productService.GetProductById(id);

    [HttpPost("CreateProduct")]
    [Authorize(Roles = "ADMIN")]
    public ResponseDto AddProduct([FromForm] ProductDto productDto) => _productService.AddProduct(productDto, HttpContext);

    [HttpPut("EditProduct")]
    [Authorize(Roles = "ADMIN")]
    public ResponseDto EditProduct([FromForm] ProductDto productDto) => _productService.EditProduct(productDto, HttpContext);

    [HttpDelete("DeleteProductbyId/{id}")]
    [Authorize(Roles = "ADMIN")]
    public ResponseDto DeleteProductbyId(Guid id) => _productService.DeleteProductById(id);
}
