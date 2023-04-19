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
    [Migration("20230419094736_Add_Product")]
    partial class Add_Product
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

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

            modelBuilder.Entity("Returns.Domain.Mock.Company", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(3)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.HasKey("Id");

                    b.ToTable("Companies", (string)null);
                });

            modelBuilder.Entity("Returns.Domain.Mock.CompanyCustomer", b =>
                {
                    b.Property<string>("CompanyId")
                        .HasMaxLength(3)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("CustomerId")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.HasKey("CompanyId", "CustomerId");

                    b.HasIndex("CustomerId");

                    b.ToTable("CompanyCustomers", (string)null);
                });

            modelBuilder.Entity("Returns.Domain.Mock.Customer", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int>("CountryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("ParentId")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("ParentId");

                    b.ToTable("Customers", (string)null);
                });

            modelBuilder.Entity("Returns.Domain.Mock.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(10)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<bool>("ByOrderOnly")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Serviceable")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Products", (string)null);
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

            modelBuilder.Entity("Returns.Domain.Mock.CompanyCustomer", b =>
                {
                    b.HasOne("Returns.Domain.Mock.Company", "Company")
                        .WithMany("Customers")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Returns.Domain.Mock.Customer", "Customer")
                        .WithMany("Companies")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Returns.Domain.Mock.Customer", b =>
                {
                    b.HasOne("Returns.Domain.Mock.Region", "Country")
                        .WithMany("Customers")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Returns.Domain.Mock.Customer", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Country");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Returns.Domain.Mock.Company", b =>
                {
                    b.Navigation("Customers");
                });

            modelBuilder.Entity("Returns.Domain.Mock.Customer", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("Companies");
                });

            modelBuilder.Entity("Returns.Domain.Mock.Region", b =>
                {
                    b.Navigation("Customers");
                });
#pragma warning restore 612, 618
        }
    }
}
