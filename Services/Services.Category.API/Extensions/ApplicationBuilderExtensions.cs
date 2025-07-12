using Microsoft.EntityFrameworkCore;
using Services.Category.API.Data;

namespace Services.Category.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseCategoryMigrationAndSeed(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (db.Database.GetPendingMigrations().Any())
            db.Database.Migrate();

        if (!db.Categories.Any())
        {
            var clothing = new Models.Category { Id = Guid.NewGuid(), Name = "Clothing" };
            var electronics = new Models.Category { Id = Guid.NewGuid(), Name = "Electronics" };

            var men = new Models.Category { Id = Guid.NewGuid(), Name = "Men", ParentCategoryId = clothing.Id };
            var women = new Models.Category { Id = Guid.NewGuid(), Name = "Women", ParentCategoryId = clothing.Id };
            var phones = new Models.Category { Id = Guid.NewGuid(), Name = "Phones", ParentCategoryId = electronics.Id };
            var laptops = new Models.Category { Id = Guid.NewGuid(), Name = "Laptops", ParentCategoryId = electronics.Id };

            db.Categories.AddRange(clothing, electronics, men, women, phones, laptops);
            db.SaveChanges();
        }
    }
}
