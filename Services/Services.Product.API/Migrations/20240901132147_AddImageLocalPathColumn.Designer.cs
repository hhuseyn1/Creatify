﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Services.Product.API.Data;

#nullable disable

namespace Services.Product.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240901132147_AddImageLocalPathColumn")]
    partial class AddImageLocalPathColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Services.Product.API.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageLocalPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("4a81d069-b0a6-4fef-afd0-4838e201c359"),
                            CategoryName = "Appetizer",
                            Description = "Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vehicula sagittis ut non lacus.",
                            ImageUrl = "https://placeholder.co/603x403",
                            Name = "Samosa",
                            Price = 15.0
                        },
                        new
                        {
                            Id = new Guid("8d0ea0f5-92dd-4c88-aae8-d44ae20232c2"),
                            CategoryName = "Appetizer",
                            Description = "Vivamus hendrerit arcu sed erat molestie vehicula.<br/> Sed vehicula erat at augue interdum posuere.",
                            ImageUrl = "https://placeholder.co/603x404",
                            Name = "Spring Roll",
                            Price = 10.0
                        },
                        new
                        {
                            Id = new Guid("1bedf3f7-3440-41d3-9c48-ef0bf075e806"),
                            CategoryName = "Main Course",
                            Description = "Maecenas vel nisi tincidunt, ullamcorper nibh a, faucibus mauris.<br/> Aenean sit amet lorem nec lorem.",
                            ImageUrl = "https://placeholder.co/603x405",
                            Name = "Chicken Tikka",
                            Price = 20.0
                        },
                        new
                        {
                            Id = new Guid("1f1d4e45-8e2c-4e2b-8461-702a03452fde"),
                            CategoryName = "Main Course",
                            Description = "Nulla facilisi. Morbi posuere, felis quis accumsan.<br/> In volutpat augue vitae vehicula.",
                            ImageUrl = "https://placeholder.co/603x406",
                            Name = "Paneer Butter Masala",
                            Price = 25.0
                        },
                        new
                        {
                            Id = new Guid("842439c1-dc75-451d-a8b2-cce94139f2af"),
                            CategoryName = "Dessert",
                            Description = "Proin auctor dolor eget libero laoreet bibendum.<br/> Phasellus ac lacus hendrerit, volutpat arcu a, vehicula nunc.",
                            ImageUrl = "https://placeholder.co/603x407",
                            Name = "Gulab Jamun",
                            Price = 12.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}