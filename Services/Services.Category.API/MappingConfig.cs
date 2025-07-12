using AutoMapper;
using Services.Category.API.Models.Dto;

namespace Services.Category.API;

public class MappingConfig
{
	public static MapperConfiguration RegisterMappings()
	{
		var mapCongif = new MapperConfiguration(config =>
		{
			config.CreateMap<Models.Category, CategoryDto>().ReverseMap();
		});
		return mapCongif;
	}
}
