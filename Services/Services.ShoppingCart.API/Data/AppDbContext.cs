using Microsoft.EntityFrameworkCore;
using Services.ShoppingCart.API.Models;

namespace Services.ShoppingCart.API.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{

	}
	public DbSet<CartHeader> CartHeaders { get; set; }
	public DbSet<CartDetails> CartDetails { get; set; }

}
