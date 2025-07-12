using Microsoft.EntityFrameworkCore;

namespace Services.Category.API.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Models.Category> Categories { get; set; } = null!;
}
