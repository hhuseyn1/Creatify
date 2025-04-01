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
        try
        {
            var shops = await _db.Shops.AsNoTracking().ToListAsync();
            return new ResponseDto
            {
                Result = _mapper.Map<List<ShopDto>>(shops),
                isSuccess = true
            };
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    public async Task<ResponseDto> GetShopByIdAsync(Guid id)
    {
        try
        {
            var shop = await _db.Shops.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            return shop == null
                ? new ResponseDto { isSuccess = false, Message = "Shop not found." }
                : new ResponseDto { Result = _mapper.Map<ShopDto>(shop), isSuccess = true };
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    public async Task<ResponseDto> GetShopByNameAsync(string name)
    {
        try
        {
            var shop = await _db.Shops.AsNoTracking().FirstOrDefaultAsync(s => s.Name == name);
            return shop == null
                ? new ResponseDto { isSuccess = false, Message = "Shop not found by name." }
                : new ResponseDto { Result = _mapper.Map<ShopDto>(shop), isSuccess = true };
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    public async Task<ResponseDto> CreateShopAsync(ShopDto shopDto)
    {
        try
        {
            var shop = _mapper.Map<Shop.API.Models.Shop>(shopDto);
            shop.Id = Guid.NewGuid();

            await _db.Shops.AddAsync(shop);
            await _db.SaveChangesAsync();

            return new ResponseDto
            {
                Result = _mapper.Map<ShopDto>(shop),
                isSuccess = true
            };
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    public async Task<ResponseDto> UpdateShopAsync(ShopDto shopDto)
    {
        try
        {
            var existingShop = await _db.Shops.FindAsync(shopDto.Id);
            if (existingShop == null)
            {
                return new ResponseDto { isSuccess = false, Message = "Shop not found." };
            }

            _mapper.Map(shopDto, existingShop);
            _db.Shops.Update(existingShop);
            await _db.SaveChangesAsync();

            return new ResponseDto
            {
                Result = _mapper.Map<ShopDto>(existingShop),
                isSuccess = true
            };
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    public async Task<ResponseDto> DeleteShopByIdAsync(Guid id)
    {
        try
        {
            var shop = await _db.Shops.FindAsync(id);
            if (shop == null)
            {
                return new ResponseDto { isSuccess = false, Message = "Shop not found." };
            }

            _db.Shops.Remove(shop);
            await _db.SaveChangesAsync();

            return new ResponseDto { Result = true, isSuccess = true };
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    private ResponseDto HandleException(Exception ex)
    {
        return new ResponseDto
        {
            isSuccess = false,
            Message = ex.Message
        };
    }
}
