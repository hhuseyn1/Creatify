using Microsoft.EntityFrameworkCore;
using Services.Email.API.Models;

namespace Services.Email.API.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{

	}
	public DbSet<EmailLogger> EmailLoggers { get; set; }
}
