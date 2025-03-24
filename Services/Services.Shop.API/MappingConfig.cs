using AutoMapper;
using Services.Shop.API.Models.Dto;

namespace Services.Shop.API;

public class MappingConfig
{
	public static MapperConfiguration RegisterMappings()
	{
		var mapCongif = new MapperConfiguration(config =>
		{
            config.CreateMap<ShopDto, Models.Shop>().ReverseMap();
        });
		return mapCongif;
	}
}
