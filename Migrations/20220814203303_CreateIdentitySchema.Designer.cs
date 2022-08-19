﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TVGuide.Models;

#nullable disable

namespace TVGuide.Migrations
{
    [DbContext(typeof(ChannelContext))]
    [Migration("20220814203303_CreateIdentitySchema")]
    partial class CreateIdentitySchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("TVGuide.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Movies"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Series"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Sports"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Documentary"
                        },
                        new
                        {
                            Id = 5,
                            Name = "News"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Kids"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Music"
                        });
                });

            modelBuilder.Entity("TVGuide.Models.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IdXML")
                        .HasColumnType("TEXT");

                    b.Property<string>("Logo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PackageId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Position")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PackageId");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("TVGuide.Models.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Packages");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "OSN"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Canal"
                        },
                        new
                        {
                            Id = 3,
                            Name = "beIN"
                        });
                });

            modelBuilder.Entity("TVGuide.Models.Channel", b =>
                {
                    b.HasOne("TVGuide.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("TVGuide.Models.Package", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId");

                    b.Navigation("Category");

                    b.Navigation("Package");
                });
#pragma warning restore 612, 618
        }
    }
}
