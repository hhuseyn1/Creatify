﻿using AutoMapper;
using Services.Product.API.Models.Dto;

namespace Services.Product.API;

public class MappingConfig
{
	public static MapperConfiguration RegisterMappings()
	{
		var mapCongif = new MapperConfiguration(config =>
		{
			config.CreateMap<ProductDto, Models.Product>().ReverseMap();
		});
		return mapCongif;
	}
}
