using AutoMapper;
using Creatify.MessageBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.ShoppingCart.API.Data;
using Services.ShoppingCart.API.Models;
using Services.ShoppingCart.API.Models.Dto;
using Services.ShoppingCart.API.Service.IService;

namespace Services.ShoppingCart.API.Controllers;

[Route("api/cart")]
[ApiController]
public class ShoppingCartAPIController : ControllerBase
{
    private ResponseDto _responseDto;
    private IMapper _mapper;
    private readonly AppDbContext _appDbContext;
    private readonly IProductService _productService;
    private readonly ICouponService _couponService;
    private readonly IMessageBus _messageBus;
    private readonly IConfiguration _configuration;
    public ShoppingCartAPIController(AppDbContext appDbContext, IMapper mapper, IProductService productService, ICouponService couponService, IMessageBus messageBus, IConfiguration configuration)
    {
        this._appDbContext = appDbContext;
        this._responseDto = new ResponseDto();
        this._mapper = mapper;
        this._productService = productService;
        this._couponService = couponService;
        this._messageBus = messageBus;
        this._configuration = configuration;
    }

    [HttpPost("GetCardbyUserId/{userId}")]
    public async Task<ResponseDto> GetCardbyUserId(Guid userId)
    {
        try
        {
            CartDto cartDto = new()
            {
               CartHeader = _mapper.Map<CartHeaderDto>(_appDbContext.CartHeaders.First(u=>u.UserId == userId))
            };
            cartDto.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_appDbContext.CartDetails.Where(u=>u.CartHeaderId == cartDto.CartHeader.Id));

            IEnumerable<ProductDto> productDtos = await _productService.GetProductsAsync();

            foreach (var item in cartDto.CartDetails)
            {
                item.Product = productDtos.FirstOrDefault(u=>u.Id == item.ProductId);
                cartDto.CartHeader.CartTotal += (item.Count * item.Product.Price);
            }

            if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
            {
                CouponDto couponDto = await _couponService.GetCouponAsync(cartDto.CartHeader.CouponCode);
                if(couponDto != null && cartDto.CartHeader.CartTotal > couponDto.MinAmount)
                {
                    cartDto.CartHeader.CartTotal -= couponDto.DiscountAmount;
                    cartDto.CartHeader.Discount  = couponDto.DiscountAmount;
                }
            }

            _responseDto.Result = cartDto;

        }
        catch (Exception ex)
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = ex.ToString();
        }
        return _responseDto;

    }

    [HttpPost("ApplyCoupon")]
    public async Task<object> ApplyCoupon([FromForm]CartDto cartDto)
    {
        try
        {
            var cartFromDb = _appDbContext.CartHeaders.First(u=>u.UserId==cartDto.CartHeader.Id);
            cartFromDb.CouponCode = cartDto.CartHeader.CouponCode;
            _appDbContext.CartHeaders.Update(cartFromDb);
            await _appDbContext.SaveChangesAsync();
            _responseDto.Result = true;

        }
        catch (Exception ex) 
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = ex.ToString();
        }
        return _responseDto;
    }

    [HttpPost("EmailCartRequest")]
    public async Task<object> EmailCartRequest([FromForm] CartDto cartDto)
    {
        try
        {
            await _messageBus.PublishMessage(cartDto,_configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"));
            _responseDto.Result = true;

        }
        catch (Exception ex)
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = ex.ToString();
        }
        return _responseDto;
    }


    [HttpPost("UpsertCart")]
    public async Task<ResponseDto> UpsertCart(CartDto cartDto)
    {
        try
        {
            var cartHeaderFromDb = await _appDbContext.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
            if (cartHeaderFromDb == null)
            {
                CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                _appDbContext.CartHeaders.Add(cartHeader);
                await _appDbContext.SaveChangesAsync();

                cartDto.CartDetails.First().CartHeaderId = cartHeader.Id;
                _appDbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                var cartDetailsFromDb = await _appDbContext.CartDetails.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.ProductId == cartDto.CartDetails
                    .First().ProductId && u.CartHeaderId == cartHeaderFromDb.Id);

                if (cartDetailsFromDb == null)
                {
                    cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.Id;
                    _appDbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _appDbContext.SaveChangesAsync();
                }
                else
                {
                    cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                    cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.Id;
                    _appDbContext.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _appDbContext.SaveChangesAsync();
                }
            }
            _responseDto.Result = cartDto;
        }
        catch (Exception ex)
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = ex.ToString();
        }
        return _responseDto;
    }


    [HttpPost("RemoveCartbyId/{cartDetailsId}")]
    public async Task<ResponseDto> RemoveCartbyId(Guid cartDetailsId)
    {
        try
        {
            CartDetails cartDetails = _appDbContext.CartDetails.First(u => u.Id == cartDetailsId);

            int totalCountCartItems = _appDbContext.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();

            _appDbContext.CartDetails.Remove(cartDetails);
            if (totalCountCartItems == 1)
            {
                var cartHeadertoRemove = await _appDbContext.CartHeaders.FirstOrDefaultAsync(u => u.Id == cartDetails.CartHeaderId);
                _appDbContext.CartHeaders.Remove(cartHeadertoRemove);
            }
            await _appDbContext.SaveChangesAsync();
            _responseDto.Result = true;
        }
        catch (Exception ex)
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = ex.ToString();
        }
        return _responseDto;
    }
}
