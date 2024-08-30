using Microsoft.EntityFrameworkCore;
using Services.Order.API.Models;

namespace Services.Order.API.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{

	}
	public DbSet<OrderHeader> OrderHeaders { get; set; }
	public DbSet<OrderDetails> OrderDetails { get; set; }

}
