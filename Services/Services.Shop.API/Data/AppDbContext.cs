using Microsoft.EntityFrameworkCore;

namespace Services.Shop.API.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{

	}
	public DbSet<Shop.API.Models.Shop> Shops { get; set; }
}
