using AutoMapper;
using Services.Coupon.API.Models.Dto;

namespace Services.Coupon.API;

public class MappingConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        var mapCongif = new MapperConfiguration(config =>
        {
            config.CreateMap<CouponDto,Models.Coupon>();
            config.CreateMap<Models.Coupon,CouponDto>();
        });
        return mapCongif;
    }
}
