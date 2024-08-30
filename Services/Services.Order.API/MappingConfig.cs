using AutoMapper;
using Services.Order.API.Models;
using Services.Order.API.Models.Dto;

namespace Services.Order.API;

public class MappingConfig
{
	public static MapperConfiguration RegisterMappings()
	{
		var mapCongif = new MapperConfiguration(config =>
		{
			config.CreateMap<OrderHeaderDto, CartHeaderDto>()
			.ForMember(dest => dest.CartTotal,u=>u.MapFrom(src=>src.OrderTotal)).ReverseMap();

			config.CreateMap<CartDetailsDto, OrderDetailsDto>()
			.ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.Product.Name))
			.ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price));

			config.CreateMap<OrderDetailsDto, CartDetailsDto>();


			config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
			config.CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();

        });
		return mapCongif;
	}
}
