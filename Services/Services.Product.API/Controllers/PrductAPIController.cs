using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Product.API.Data;
using Services.Product.API.Models.Dto;
using Services.Product.API.Models.DTOs;


namespace Services.Product.API.Controllers;

[Route("api/product")]
[ApiController]
[Authorize]
public class PrductAPIController : ControllerBase
{
    private readonly AppDbContext _context;
    private ResponseDto _response;
    public IMapper _mapper;

    public PrductAPIController(AppDbContext context, IMapper mapper)
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
            IEnumerable<Models.Product> objList = _context.Products.ToList();
            _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
        }
        catch (Exception ex)
        {
            _response.isSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public ResponseDto GetProductbyId(Guid id)
    {
        try
        {
            Models.Product obj = _context.Products.First(u=>u.Id == id);
            _response.Result = _mapper.Map<ProductDto>(obj);
        }
        catch (Exception ex)
        {
            _response.isSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    
    [HttpPost]
    [Route("AddProduct")]
    public ResponseDto AddProduct([FromBody] ProductDto ProductDto)
    {
        try
        {
            Models.Product obj = _mapper.Map<Models.Product>(ProductDto);
            _context.Add(obj);
            _context.SaveChanges();

            _response.Result = _mapper.Map<ProductDto>(obj);
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
    public ResponseDto EditProductbyId([FromBody] ProductDto ProductDto)
    {
        try
        {
            Models.Product obj = _mapper.Map<Models.Product>(ProductDto);
            _context.Update(obj);
            _context.SaveChanges();

            _response.Result = _mapper.Map<ProductDto>(obj);
        }
        catch (Exception ex)
        {
            _response.isSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    [HttpDelete]
    [Route("DeleteProductbyId/{id}")]
    public ResponseDto DeleteProductbyId(Guid id)
    {
        try
        {
            Models.Product obj = _context.Products.First(u => u.Id == id);
            _context.Remove(obj);
            _context.SaveChanges();

            _response.Result = _mapper.Map<ProductDto>(obj);
        }
        catch (Exception ex)
        {
            _response.isSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

}
