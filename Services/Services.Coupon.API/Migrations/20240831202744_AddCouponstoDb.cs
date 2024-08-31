using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Services.Coupon.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCouponstoDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CouponCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    MinAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "CouponCode", "DiscountAmount", "MinAmount" },
                values: new object[,]
                {
                    { new Guid("c0e3ea01-20a7-4d70-a29e-f486009db63b"), "10OFD", 30.0, 60 },
                    { new Guid("ec061225-aa8a-4967-bff8-abf5aa28256e"), "10OFF", 10.0, 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
