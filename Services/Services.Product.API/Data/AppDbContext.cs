using Microsoft.EntityFrameworkCore;

namespace Services.Product.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Models.Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Models.Product>()
            .HasOne(p => p.MainCategory)
            .WithMany()
            .HasForeignKey(p => p.MainCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Models.Product>()
            .HasOne(p => p.SubCategory)
            .WithMany()
            .HasForeignKey(p => p.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
