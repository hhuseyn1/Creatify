using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services.Shop.API.Data;
using Services.Shop.API.Models.Dto;
using Services.Shop.API.Service.IService;

namespace Services.Shop.API.Service;

public class ShopService : IShopService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public ShopService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ResponseDto> GetAllShopsAsync()
    {
        var response = new ResponseDto();
        try
        {
            var shops = await _db.Shops.ToListAsync();
            response.Result = _mapper.Map<List<ShopDto>>(shops);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDto> GetShopByIdAsync(Guid id)
    {
        var response = new ResponseDto();
        try
        {
            var shop = await _db.Shops.FindAsync(id);
            if (shop == null)
            {
                response.isSuccess = false;
                response.Message = "Shop not found.";
                return response;
            }

            response.Result = _mapper.Map<ShopDto>(shop);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDto> GetShopByNameAsync(string name)
    {
        var response = new ResponseDto();
        try
        {
            var shop = await _db.Shops
                .FirstOrDefaultAsync(s => s.Name == name);

            if (shop == null)
            {
                response.isSuccess = false;
                response.Message = "Shop not found by name.";
                return response;
            }

            response.Result = _mapper.Map<ShopDto>(shop);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDto> CreateShopAsync(ShopDto shopDto)
    {
        var response = new ResponseDto();
        try
        {
            var shop = _mapper.Map<Shop.API.Models.Shop>(shopDto);
            shop.Id = Guid.NewGuid();

            await _db.Shops.AddAsync(shop);
            await _db.SaveChangesAsync();

            response.Result = _mapper.Map<ShopDto>(shop);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDto> UpdateShopAsync(ShopDto shopDto)
    {
        var response = new ResponseDto();
        try
        {
            var shopFromDb = await _db.Shops.FindAsync(shopDto.Id);
            if (shopFromDb == null)
            {
                response.isSuccess = false;
                response.Message = "Shop not found.";
                return response;
            }

            shopFromDb.Name = shopDto.Name;
            shopFromDb.OwnerEmail = shopDto.OwnerEmail;
            shopFromDb.ContactEmail = shopDto.ContactEmail;
            shopFromDb.PhoneNumber = shopDto.PhoneNumber;
            shopFromDb.Location = shopDto.Location;
            shopFromDb.Description = shopDto.Description;
            shopFromDb.ImageUrl = shopDto.ImageUrl;
            shopFromDb.ImageLocalPath = shopDto.ImageLocalPath;

            _db.Shops.Update(shopFromDb);
            await _db.SaveChangesAsync();

            response.Result = _mapper.Map<ShopDto>(shopFromDb);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResponseDto> DeleteShopByIdAsync(Guid id)
    {
        var response = new ResponseDto();
        try
        {
            var shop = await _db.Shops.FindAsync(id);
            if (shop == null)
            {
                response.isSuccess = false;
                response.Message = "Shop not found.";
                return response;
            }

            _db.Shops.Remove(shop);
            await _db.SaveChangesAsync();
            response.Result = true;
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }
}
