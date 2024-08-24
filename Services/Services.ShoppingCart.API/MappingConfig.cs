using AutoMapper;
using Services.ShoppingCart.API.Models;
using Services.ShoppingCart.API.Models.Dto;

namespace Services.ShoppingCart.API;

public class MappingConfig
{
	public static MapperConfiguration RegisterMappings()
	{
		var mapCongif = new MapperConfiguration(config =>
		{
			config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
			config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
		});
		return mapCongif;
	}
}
