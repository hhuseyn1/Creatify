using AutoMapper;
using Services.Product.API.Data;
using Services.Product.API.Models.Dto;
using Services.Product.API.Service.IService;

namespace Services.Product.API.Service;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProductService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public ResponseDto GetAllProducts()
    {
        var response = new ResponseDto();
        try
        {
            IEnumerable<Models.Product> productList = _context.Products.ToList();
            response.Result = _mapper.Map<IEnumerable<ProductDto>>(productList);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public ResponseDto GetProductById(Guid id)
    {
        var response = new ResponseDto();
        try
        {
            Models.Product product = _context.Products.First(u => u.Id == id);
            response.Result = _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public ResponseDto AddProduct(ProductDto productDto, HttpContext httpContext)
    {
        var response = new ResponseDto();
        try
        {
            Models.Product product = _mapper.Map<Models.Product>(productDto);
            _context.Add(product);
            _context.SaveChanges();

            HandleImageUpload(productDto, product, httpContext);

            _context.Products.Update(product);
            _context.SaveChanges();

            response.Result = _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public ResponseDto EditProduct(ProductDto productDto, HttpContext httpContext)
    {
        var response = new ResponseDto();
        try
        {
            Models.Product product = _mapper.Map<Models.Product>(productDto);

            if (productDto.Image != null)
            {
                DeleteOldImage(product);
                HandleImageUpload(productDto, product, httpContext);
            }

            _context.Update(product);
            _context.SaveChanges();

            response.Result = _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public ResponseDto DeleteProductById(Guid id)
    {
        var response = new ResponseDto();
        try
        {
            Models.Product product = _context.Products.First(u => u.Id == id);
            DeleteOldImage(product);
            _context.Remove(product);
            _context.SaveChanges();
            response.Result = _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    private void HandleImageUpload(ProductDto productDto, Models.Product product, HttpContext httpContext)
    {
        if (productDto.Image != null)
        {
            string fileName = product.Id + Path.GetExtension(productDto.Image.FileName);
            string filePath = @"wwwroot\ProductImages\" + fileName;
            var filePathDir = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            using (var fileStream = new FileStream(filePathDir, FileMode.Create))
            {
                productDto.Image.CopyTo(fileStream);
            }

            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}{httpContext.Request.PathBase.Value}";
            product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
            product.ImageLocalPath = filePath;
        }
        else
        {
            product.ImageUrl = "https://placehold.co/600x400";
        }
    }

    private void DeleteOldImage(Models.Product product)
    {
        if (!string.IsNullOrEmpty(product.ImageLocalPath))
        {
            var oldFilePathDir = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
            FileInfo oldFile = new FileInfo(oldFilePathDir);
            if (oldFile.Exists)
                oldFile.Delete();
        }
    }
}
