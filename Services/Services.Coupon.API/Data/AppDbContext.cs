using Microsoft.EntityFrameworkCore;

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
			Id = Guid.NewGuid(),
			CouponCode = "10OFF",
			DiscountAmount = 10,
			MinAmount = 20
		});

		modelBuilder.Entity<Models.Coupon>().HasData(new Models.Coupon
		{
			Id = Guid.NewGuid(),
			CouponCode = "10OFD",
			DiscountAmount = 30,
			MinAmount = 60
		});
	}

}
