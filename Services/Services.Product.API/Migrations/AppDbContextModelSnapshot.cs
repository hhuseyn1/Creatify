﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Services.Product.API.Data;

#nullable disable

namespace Services.Product.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("ImageUrl")
                        .IsRequired()
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
                            Id = new Guid("8582c3bc-f092-4e1e-a2d5-69b5d794bd63"),
                            CategoryName = "Appetizer",
                            Description = "Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vehicula sagittis ut non lacus.",
                            ImageUrl = "https://placeholder.co/603x403",
                            Name = "Samosa",
                            Price = 15.0
                        },
                        new
                        {
                            Id = new Guid("593d79b0-84b8-43c6-bca0-57a39ec3426e"),
                            CategoryName = "Appetizer",
                            Description = "Vivamus hendrerit arcu sed erat molestie vehicula.<br/> Sed vehicula erat at augue interdum posuere.",
                            ImageUrl = "https://placeholder.co/603x404",
                            Name = "Spring Roll",
                            Price = 10.0
                        },
                        new
                        {
                            Id = new Guid("33d259cd-f7a8-4eba-aa89-5caf28b7581f"),
                            CategoryName = "Main Course",
                            Description = "Maecenas vel nisi tincidunt, ullamcorper nibh a, faucibus mauris.<br/> Aenean sit amet lorem nec lorem.",
                            ImageUrl = "https://placeholder.co/603x405",
                            Name = "Chicken Tikka",
                            Price = 20.0
                        },
                        new
                        {
                            Id = new Guid("8c29aa8e-f9a3-4aba-95b7-bb63e52eff84"),
                            CategoryName = "Main Course",
                            Description = "Nulla facilisi. Morbi posuere, felis quis accumsan.<br/> In volutpat augue vitae vehicula.",
                            ImageUrl = "https://placeholder.co/603x406",
                            Name = "Paneer Butter Masala",
                            Price = 25.0
                        },
                        new
                        {
                            Id = new Guid("d5b4e258-cb0d-4aad-9c56-6b1b7d42c728"),
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