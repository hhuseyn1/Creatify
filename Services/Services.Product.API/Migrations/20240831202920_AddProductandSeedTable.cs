using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Services.Product.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProductandSeedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryName", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("13fdd1ce-4769-44c4-aef5-20be86fceffe"), "Dessert", "Proin auctor dolor eget libero laoreet bibendum.<br/> Phasellus ac lacus hendrerit, volutpat arcu a, vehicula nunc.", "https://placeholder.co/603x407", "Gulab Jamun", 12.0 },
                    { new Guid("23104283-c610-4493-98f9-82bbe95de8f2"), "Main Course", "Maecenas vel nisi tincidunt, ullamcorper nibh a, faucibus mauris.<br/> Aenean sit amet lorem nec lorem.", "https://placeholder.co/603x405", "Chicken Tikka", 20.0 },
                    { new Guid("58e4a7e1-cccd-452f-8fe8-8c2708a8658f"), "Main Course", "Nulla facilisi. Morbi posuere, felis quis accumsan.<br/> In volutpat augue vitae vehicula.", "https://placeholder.co/603x406", "Paneer Butter Masala", 25.0 },
                    { new Guid("6c191b4a-9242-43ba-8724-0fff01c97185"), "Appetizer", "Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vehicula sagittis ut non lacus.", "https://placeholder.co/603x403", "Samosa", 15.0 },
                    { new Guid("f16fb1c5-8426-43df-9188-db11ba24042f"), "Appetizer", "Vivamus hendrerit arcu sed erat molestie vehicula.<br/> Sed vehicula erat at augue interdum posuere.", "https://placeholder.co/603x404", "Spring Roll", 10.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
