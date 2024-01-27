using Microsoft.EntityFrameworkCore;
using Services.Coupon.API.Models;

namespace Services.Coupon.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Models.Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Models.Coupon>().HasData(new Models.Coupon
        {
            Id=1,
            CouponCode = "10OFF",
            DiscountAmount=10,
            MinAmount=20
        });

        modelBuilder.Entity<Models.Coupon>().HasData(new Models.Coupon
        {
            Id = 2,
            CouponCode = "10OFF",
            DiscountAmount = 30,
            MinAmount = 60
        });
    }

}
