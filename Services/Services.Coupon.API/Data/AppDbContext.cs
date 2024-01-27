using Microsoft.EntityFrameworkCore;
using Services.Coupon.API.Models;

namespace Services.Coupon.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Models.Coupon> Coupons { get; set; }
}
