﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Services.Product.API.Migrations
{
    /// <inheritdoc />
    public partial class AddImageLocalPathColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("13fdd1ce-4769-44c4-aef5-20be86fceffe"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("23104283-c610-4493-98f9-82bbe95de8f2"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("58e4a7e1-cccd-452f-8fe8-8c2708a8658f"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("6c191b4a-9242-43ba-8724-0fff01c97185"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f16fb1c5-8426-43df-9188-db11ba24042f"));

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ImageLocalPath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryName", "Description", "ImageLocalPath", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("1bedf3f7-3440-41d3-9c48-ef0bf075e806"), "Main Course", "Maecenas vel nisi tincidunt, ullamcorper nibh a, faucibus mauris.<br/> Aenean sit amet lorem nec lorem.", null, "https://placeholder.co/603x405", "Chicken Tikka", 20.0 },
                    { new Guid("1f1d4e45-8e2c-4e2b-8461-702a03452fde"), "Main Course", "Nulla facilisi. Morbi posuere, felis quis accumsan.<br/> In volutpat augue vitae vehicula.", null, "https://placeholder.co/603x406", "Paneer Butter Masala", 25.0 },
                    { new Guid("4a81d069-b0a6-4fef-afd0-4838e201c359"), "Appetizer", "Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vehicula sagittis ut non lacus.", null, "https://placeholder.co/603x403", "Samosa", 15.0 },
                    { new Guid("842439c1-dc75-451d-a8b2-cce94139f2af"), "Dessert", "Proin auctor dolor eget libero laoreet bibendum.<br/> Phasellus ac lacus hendrerit, volutpat arcu a, vehicula nunc.", null, "https://placeholder.co/603x407", "Gulab Jamun", 12.0 },
                    { new Guid("8d0ea0f5-92dd-4c88-aae8-d44ae20232c2"), "Appetizer", "Vivamus hendrerit arcu sed erat molestie vehicula.<br/> Sed vehicula erat at augue interdum posuere.", null, "https://placeholder.co/603x404", "Spring Roll", 10.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("1bedf3f7-3440-41d3-9c48-ef0bf075e806"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("1f1d4e45-8e2c-4e2b-8461-702a03452fde"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("4a81d069-b0a6-4fef-afd0-4838e201c359"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("842439c1-dc75-451d-a8b2-cce94139f2af"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("8d0ea0f5-92dd-4c88-aae8-d44ae20232c2"));

            migrationBuilder.DropColumn(
                name: "ImageLocalPath",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
