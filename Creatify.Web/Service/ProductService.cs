﻿using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Creatify.Web.Utility;

namespace Creatify.Web.Service;

public class ProductService : IProductService
{
	private readonly IBaseService _baseService;

	public ProductService(IBaseService baseService)
	{
		_baseService = baseService;
	}
	public async Task<ResponseDto> GetAllProductsAsync()
	{
		return await _baseService.SendAsync(new()
		{
			APIType = StaticDetails.APIType.GET,
			Url = StaticDetails.ProductAPIBase + "/api/Product/GetAllProducts"
		});
	}

	public async Task<ResponseDto> GetProductByIdAsync(Guid id)
	{
		return await _baseService.SendAsync(new()
		{
			APIType = StaticDetails.APIType.GET,
			Url = StaticDetails.ProductAPIBase + "/api/Product/GetProductById/" + id
		});
	}

	public async Task<ResponseDto> CreateProductAsync(ProductDto ProductDto)
	{
		return await _baseService.SendAsync(new()
		{
			APIType = StaticDetails.APIType.POST,
			Data = ProductDto,
			Url = StaticDetails.ProductAPIBase + "/api/Product/CreateProduct",
			ContentType	= StaticDetails.ContentType.MultipartFormData
		});
	}

	public async Task<ResponseDto> DeleteProductAsync(Guid id)
	{
		return await _baseService.SendAsync(new()
		{
			APIType = StaticDetails.APIType.DELETE,
			Url = StaticDetails.ProductAPIBase + "/api/Product/DeleteProductbyId/" + id
		});
	}

	public async Task<ResponseDto> UpdateProductAsync(ProductDto ProductDto)
	{
		return await _baseService.SendAsync(new()
		{
			APIType = StaticDetails.APIType.PUT,
			Data = ProductDto,
			Url = StaticDetails.ProductAPIBase + "/api/Product/EditProduct",
            ContentType = StaticDetails.ContentType.MultipartFormData
        });
	}
}
