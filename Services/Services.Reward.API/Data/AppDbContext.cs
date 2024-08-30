using Microsoft.EntityFrameworkCore;
using Services.Reward.API.Models;

namespace Services.Reward.API.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{

	}
	public DbSet<Rewards> Rewards { get; set; }

}
