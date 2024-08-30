using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Reward.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRewardTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RewardsDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RewardsActivity = table.Column<int>(type: "int", nullable: false),
                    orderId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rewards");
        }
    }
}
