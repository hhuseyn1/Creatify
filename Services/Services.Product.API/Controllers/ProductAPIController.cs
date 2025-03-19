using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Product.API.Data;
using Services.Product.API.Models.Dto;

namespace Services.Product.API.Controllers;

[Route("api/product")]
[ApiController]
public class ProductAPIController : ControllerBase
{
    private readonly AppDbContext _context;
    private ResponseDto _response;
    public IMapper _mapper;

    public ProductAPIController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _response = new ResponseDto();
        _mapper = mapper;
    }

    [HttpGet]
    [Route("GetAllProducts")]
    public ResponseDto GetAllProducts()
    {
        try
        {
            IEnumerable<Models.Product> productList = _context.Products.ToList();
            _response.Result = _mapper.Map<IEnumerable<ProductDto>>(productList);
        }
        catch (Exception ex)
        {
            _response.isSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    [HttpGet]
    [Route("GetProductById/{id}")]
    public ResponseDto GetProductById(Guid id)
    {
        try
        {
            Models.Product product = _context.Products.First(u => u.Id == id);
            _response.Result = _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            _response.isSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    [HttpPost]
    [Route("CreateProduct")]
    [Authorize(Roles = "ADMIN")]
    public ResponseDto AddProduct(ProductDto productDto)
    {
        try
        {
            Models.Product product = _mapper.Map<Models.Product>(productDto);
            _context.Add(product);
            _context.SaveChanges();

            if (productDto.Image != null)
            {
                string fileName = product.Id + Path.GetExtension(productDto.Image.FileName);
                string filePath = @"wwwroot\ProductImages\" + fileName;
                var filePathDir = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                using (var fileStream = new FileStream(filePathDir, FileMode.Create))
                {
                    productDto.Image.CopyTo(fileStream);
                }

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                product.ImageLocalPath = filePath;
            }
            else
            {
                product.ImageUrl = "https://placehold.co/600x400";
            }

            _context.Products.Update(product);
            _context.SaveChanges();

            _response.Result = _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            _response.isSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    [HttpPut]
    [Route("EditProduct")]
    [Authorize(Roles = "ADMIN")]
    public ResponseDto EditProductbyId(ProductDto productDto)
    {
        try
        {
            Models.Product product = _mapper.Map<Models.Product>(productDto);

            if (productDto.Image != null)
            {

                if (!string.IsNullOrEmpty(product.ImageLocalPath))
                {
                    var oldFilePathDir = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                    FileInfo oldFile = new FileInfo(oldFilePathDir);
                    if (oldFile.Exists)
                        oldFile.Delete();
                }

                string fileName = product.Id + Path.GetExtension(productDto.Image.FileName);
                string filePath = @"wwwroot\ProductImages\" + fileName;
                var filePathDir = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                using (var fileStream = new FileStream(filePathDir, FileMode.Create))
                {
                    productDto.Image.CopyTo(fileStream);
                }

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                product.ImageLocalPath = filePath;
            }

            _context.Update(product);
            _context.SaveChanges();

            _response.Result = _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            _response.isSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    [HttpDelete]
    [Authorize(Roles = "ADMIN")]
    [Route("DeleteProductbyId/{id}")]
    public ResponseDto DeleteProductbyId(Guid id)
    {
        try
        {
            Models.Product product = _context.Products.First(u => u.Id == id);

            if (!string.IsNullOrEmpty(product.ImageLocalPath))
            {
                var oldFilePathDir = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                FileInfo oldFile = new FileInfo(oldFilePathDir);
                if (oldFile.Exists)
                    oldFile.Delete();
            }

            _context.Remove(product);
            _context.SaveChanges();

            _response.Result = _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            _response.isSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }
}
