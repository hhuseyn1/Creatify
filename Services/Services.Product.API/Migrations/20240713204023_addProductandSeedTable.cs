using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Services.Product.API.Migrations
{
	/// <inheritdoc />
	public partial class addProductandSeedTable : Migration
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
					{ new Guid("33d259cd-f7a8-4eba-aa89-5caf28b7581f"), "Main Course", "Maecenas vel nisi tincidunt, ullamcorper nibh a, faucibus mauris.<br/> Aenean sit amet lorem nec lorem.", "https://placeholder.co/603x405", "Chicken Tikka", 20.0 },
					{ new Guid("593d79b0-84b8-43c6-bca0-57a39ec3426e"), "Appetizer", "Vivamus hendrerit arcu sed erat molestie vehicula.<br/> Sed vehicula erat at augue interdum posuere.", "https://placeholder.co/603x404", "Spring Roll", 10.0 },
					{ new Guid("8582c3bc-f092-4e1e-a2d5-69b5d794bd63"), "Appetizer", "Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vehicula sagittis ut non lacus.", "https://placeholder.co/603x403", "Samosa", 15.0 },
					{ new Guid("8c29aa8e-f9a3-4aba-95b7-bb63e52eff84"), "Main Course", "Nulla facilisi. Morbi posuere, felis quis accumsan.<br/> In volutpat augue vitae vehicula.", "https://placeholder.co/603x406", "Paneer Butter Masala", 25.0 },
					{ new Guid("d5b4e258-cb0d-4aad-9c56-6b1b7d42c728"), "Dessert", "Proin auctor dolor eget libero laoreet bibendum.<br/> Phasellus ac lacus hendrerit, volutpat arcu a, vehicula nunc.", "https://placeholder.co/603x407", "Gulab Jamun", 12.0 }
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
