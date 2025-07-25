using Creatify.Shared.Models;
using Creatify.Shared.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Services.User.API.Data;
using Services.User.API.Models.Dto;
using Services.User.API.Services.IServices;

namespace Services.User.API.Services;

public class AddressService : IAddressService
{
    private readonly AppDbContext _context;

    public AddressService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AddressDto>> GetUserAddressesAsync(Guid userId)
    {
        var addresses = await _context.Addresses
            .Where(a => a.UserId == userId)
            .Select(a => new AddressDto
            {
                Id = a.Id,
                Line1 = a.Line1,
                City = a.City,
                PostalCode = a.PostalCode
            })
            .ToListAsync();

        return addresses;
    }

    public async Task<bool> CreateAddressAsync(Guid userId, CreateAddressDto dto)
    {
        var existingCount = await _context.Addresses.CountAsync(a => a.UserId == userId);
        if (existingCount >= 5)
            return false;

        var address = new Address
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Line1 = dto.Line1,
            City = dto.City,
            PostalCode = dto.PostalCode,
            CreatedAt = DateTime.UtcNow
        };

        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAddressAsync(Guid addressId, Guid userId, CreateAddressDto dto)
    {
        var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);
        if (address == null)
            return false;

        address.Line1 = dto.Line1;
        address.City = dto.City;
        address.PostalCode = dto.PostalCode;

        _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAddressAsync(Guid addressId, Guid userId)
    {
        var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);
        if (address == null)
            return false;

        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
        return true;
    }
}
