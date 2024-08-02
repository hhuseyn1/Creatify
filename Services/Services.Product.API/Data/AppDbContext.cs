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

        modelBuilder.Entity<Models.Product>().HasData(
        new Models.Product
        {
            Id = Guid.NewGuid(),
            Name = "Samosa",
            Price = 15,
            Description = "Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vehicula sagittis ut non lacus.",
            ImageUrl = "https://placeholder.co/603x403",
            CategoryName = "Appetizer"
        },
        new Models.Product
        {
            Id = Guid.NewGuid(),
            Name = "Spring Roll",
            Price = 10,
            Description = "Vivamus hendrerit arcu sed erat molestie vehicula.<br/> Sed vehicula erat at augue interdum posuere.",
            ImageUrl = "https://placeholder.co/603x404",
            CategoryName = "Appetizer"
        },
        new Models.Product
        {
            Id = Guid.NewGuid(),
            Name = "Chicken Tikka",
            Price = 20,
            Description = "Maecenas vel nisi tincidunt, ullamcorper nibh a, faucibus mauris.<br/> Aenean sit amet lorem nec lorem.",
            ImageUrl = "https://placeholder.co/603x405",
            CategoryName = "Main Course"
        },
        new Models.Product
        {
            Id = Guid.NewGuid(),
            Name = "Paneer Butter Masala",
            Price = 25,
            Description = "Nulla facilisi. Morbi posuere, felis quis accumsan.<br/> In volutpat augue vitae vehicula.",
            ImageUrl = "https://placeholder.co/603x406",
            CategoryName = "Main Course"
        },
        new Models.Product
        {
            Id = Guid.NewGuid(),
            Name = "Gulab Jamun",
            Price = 12,
            Description = "Proin auctor dolor eget libero laoreet bibendum.<br/> Phasellus ac lacus hendrerit, volutpat arcu a, vehicula nunc.",
            ImageUrl = "https://placeholder.co/603x407",
            CategoryName = "Dessert"
        }
    );

    }

}
