﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Returns.Logic.Mock.Repositories;

#nullable disable

namespace Returns.Logic.Mock.Migrations
{
    [DbContext(typeof(MockDbContext))]
    [Migration("20230408154049_Add_Region")]
    partial class Add_Region
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("RegionRegion", b =>
                {
                    b.Property<int>("ChildrenId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParentsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ChildrenId", "ParentsId");

                    b.HasIndex("ParentsId");

                    b.ToTable("RegionRegion");
                });

            modelBuilder.Entity("Returns.Domain.Mock.Region", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Regions", (string)null);
                });

            modelBuilder.Entity("RegionRegion", b =>
                {
                    b.HasOne("Returns.Domain.Mock.Region", null)
                        .WithMany()
                        .HasForeignKey("ChildrenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Returns.Domain.Mock.Region", null)
                        .WithMany()
                        .HasForeignKey("ParentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
