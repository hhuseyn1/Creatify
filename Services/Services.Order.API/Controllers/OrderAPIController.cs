using AutoMapper;
using Creatify.MessageBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Order.API.Data;
using Services.Order.API.Models;
using Services.Order.API.Models.Dto;
using Services.Order.API.Service.IService;
using Services.Order.API.Utility;
using Stripe;
using Stripe.Checkout;
using System.Collections.Concurrent;

namespace Services.Order.API.Controllers;

[Route("api/order")]
[ApiController]
public class OrderAPIController : ControllerBase
{
    protected ResponseDto _responseDto;
    private IMapper _mapper;
    private readonly AppDbContext _context;
    private IProductService _productService;
    private readonly IMessageBus _messageBus;
    private readonly IConfiguration _configuration;
    public OrderAPIController(IMapper mapper, AppDbContext context, IProductService productService, IMessageBus messageBus, IConfiguration configuration)
    {
        this._mapper = mapper;
        this._context = context;
        this._messageBus = messageBus;
        this._productService = productService;
        _responseDto = new ResponseDto();
        this._configuration = configuration;
    }


    [Authorize]
    [HttpGet("GetAllOrdersbyUserId/{userId}")]
    public ResponseDto? GetAllOrdersbyUserId(Guid userId)
    {
        try
        {
            IEnumerable<OrderHeader> objList;
            if (User.IsInRole(StaticDetails.RoleAdmin))
            {
                objList = _context.OrderHeaders.Include(x => x.OrderDetails).OrderByDescending(u => u.Id).ToList();
            }
            else
            {
                objList = _context.OrderHeaders.Include(x => x.OrderDetails).Where(u => u.UserId == userId).OrderByDescending(u => u.Id).ToList();
            }
            _responseDto.Result = _mapper.Map<IEnumerable<OrderHeaderDto>>(objList);
        }
        catch (Exception ex)
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = ex.Message.ToString();
        }
        return _responseDto;
    }

    [Authorize]
    [HttpGet("GetOrdersbyId/{id}")]
    public ResponseDto? GetOrdersbyId(Guid id)
    {
        try
        {
            OrderHeader orderHeader = _context.OrderHeaders.Include(u => u.OrderDetails).First(u => u.Id == id);
            _responseDto.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
        }
        catch (Exception ex)
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = ex.Message.ToString();
        }
        return _responseDto;
    }


    [Authorize]
    [HttpPost("CreateOrder")]
    public async Task<ResponseDto> CreateOrder([FromBody] CartDto cartDto)
    {
        try
        {
            OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
            orderHeaderDto.OrderTime = DateTime.Now;
            orderHeaderDto.Status = StaticDetails.Status_Pending;
            orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);

            OrderHeader orderCreated = _context.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
            await _context.SaveChangesAsync();

            orderHeaderDto.OrderHeaderId = orderCreated.Id;
            _responseDto.Result = orderHeaderDto;
        }
        catch (Exception ex)
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = ex.Message.ToString();
        }
        return _responseDto;
    }

    [Authorize]
    [HttpPost("CreateStripeSession")]
    public async Task<ResponseDto> CreateStripeSession([FromBody] StripeRequestDto requestDto)
    {
        try
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = requestDto.ApprovedUrl,
                CancelUrl = requestDto.CancelUrl,
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
            };

            var discountsObj = new List<SessionDiscountOptions>()
            {
                new SessionDiscountOptions
                {
                    Coupon =  requestDto.OrderHeader.CouponCode
                }
            };


            foreach (var item in requestDto.OrderHeader.OrderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }


            if (requestDto.OrderHeader.Discount > 0)
            {
                options.Discounts = discountsObj;
            }

            var service = new Stripe.Checkout.SessionService();
            Session session = service.Create(options);
            requestDto.StripeSessionUrl = session.Url;
            OrderHeader orderHeader = _context.OrderHeaders.First(u => u.Id == requestDto.OrderHeader.OrderHeaderId);
            orderHeader.StripeSessionId = Guid.Parse(session.Id);
            _context.SaveChanges();
            _responseDto.Result = requestDto;
        }
        catch (Exception ex)
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = ex.Message.ToString();
        }
        return _responseDto;
    }

    [Authorize]
    [HttpPost("ValidateStripeSession")]
    public async Task<ResponseDto> ValidateStripeSession([FromBody] Guid orderHeaderId)
    {
        try
        {
            OrderHeader orderHeader = _context.OrderHeaders.First(u => orderHeaderId == u.Id);
            var service = new Stripe.Checkout.SessionService();
            Session session = service.Get(orderHeader.StripeSessionId.ToString());

            var paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

            if (paymentIntent.Status == "succeeded")
            {
                orderHeader.PaymentIntentId = Guid.Parse(paymentIntent.Id);
                orderHeader.Status = StaticDetails.Status_Approved;
                _context.SaveChanges();

                RewardsDto rewardsDto = new()
                {
                    OrderId = orderHeader.Id,
                    RewardsActivity = Convert.ToInt32(orderHeader.OrderTotal),
                    UserId = orderHeader.UserId
                };
                string topicName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
                await _messageBus.PublishMessage(rewardsDto, topicName);
                _responseDto.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
            }

        }
        catch (Exception ex)
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = ex.Message.ToString();
        }
        return _responseDto;
    }


    [Authorize]
    [HttpPut("UpdateOrderStatusbyId/{orderId}")]
    public async Task<ResponseDto> UpdateOrderStatusbyId(Guid orderId, [FromBody] string newStatus)
    {
        try
        {
            OrderHeader orderHeader = _context.OrderHeaders.First(u => u.Id == orderId);
            if (orderHeader != null)
            {
                if (newStatus == StaticDetails.Status_Cancelled)
                {
                    var options = new RefundCreateOptions
                    {
                        Reason = RefundReasons.RequestedByCustomer,
                        PaymentIntent = orderHeader.PaymentIntentId.ToString()
                    };

                    var service = new RefundService();
                    Refund refund = service.Create(options);
                }
                orderHeader.Status = newStatus;
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            _responseDto.isSuccess = false;
            _responseDto.Message = ex.Message.ToString();
        }
        return _responseDto;
    }
}
