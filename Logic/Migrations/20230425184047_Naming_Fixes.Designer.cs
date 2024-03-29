﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Returns.Logic.Repositories;

#nullable disable

namespace Returns.Logic.Migrations
{
    [DbContext(typeof(ReturnDbContext))]
    [Migration("20230425184047_Naming_Fixes")]
    partial class Naming_Fixes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Returns.Domain.Entities.FeeConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CustomerId")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<int>("FeeConfigurationGroupId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("TEXT");

                    b.Property<int?>("RegionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserCreated")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("UserModified")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<decimal>("Value")
                        .HasPrecision(18, 4)
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("ValueMinimum")
                        .HasPrecision(18, 4)
                        .HasColumnType("TEXT");

                    b.Property<int>("ValueType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FeeConfigurationGroupId");

                    b.HasIndex("CustomerId", "FeeConfigurationGroupId", "RegionId")
                        .IsUnique()
                        .HasFilter("[Deleted] = 0");

                    b.ToTable("FeeConfigurations", null, t =>
                        {
                            t.HasCheckConstraint("CK_FeeConfigurations_RegionId_CustomerId", "[RegionId] IS NULL OR [CustomerId] IS NULL");
                        });
                });

            modelBuilder.Entity("Returns.Domain.Entities.FeeConfigurationGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyId")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int?>("DelayDays")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("FeeConfigurationGroups", (string)null);
                });

            modelBuilder.Entity("Returns.Domain.Entities.Return", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyId")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("DeliveryPointId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int>("LabelCount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserCreated")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("UserModified")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId", "CustomerId");

                    b.HasIndex("CompanyId", "Number")
                        .IsUnique();

                    b.ToTable("Returns", (string)null);
                });

            modelBuilder.Entity("Returns.Domain.Entities.ReturnAvailability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyId")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int>("Days")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RegionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId", "RegionId")
                        .IsUnique();

                    b.ToTable("ReturnAvailabilities", (string)null);
                });

            modelBuilder.Entity("Returns.Domain.Entities.ReturnFee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<int>("FeeConfigurationId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReturnId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ReturnLineId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserCreated")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("UserModified")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<decimal?>("Value")
                        .HasPrecision(18, 4)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FeeConfigurationId");

                    b.HasIndex("ReturnId");

                    b.HasIndex("ReturnLineId");

                    b.ToTable("ReturnFees", (string)null);
                });

            modelBuilder.Entity("Returns.Domain.Entities.ReturnLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("InvoiceNumberPurchase")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("InvoiceNumberReturn")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("TEXT");

                    b.Property<string>("NoteResponse")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("NoteReturn")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<decimal>("PriceUnit")
                        .HasPrecision(18, 4)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int>("ProductType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ReturnId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SerialNumber")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserCreated")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("UserModified")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.HasKey("Id");

                    b.HasIndex("ReturnId");

                    b.HasIndex("InvoiceNumberPurchase", "ProductId");

                    b.ToTable("ReturnLines", (string)null);
                });

            modelBuilder.Entity("Returns.Domain.Entities.ReturnLineAttachment", b =>
                {
                    b.Property<Guid>("StorageId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int>("ReturnLineId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserCreated")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<string>("UserModified")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.HasKey("StorageId");

                    b.HasIndex("ReturnLineId");

                    b.ToTable("ReturnLineAttachments", (string)null);
                });

            modelBuilder.Entity("Returns.Domain.Entities.FeeConfiguration", b =>
                {
                    b.HasOne("Returns.Domain.Entities.FeeConfigurationGroup", "Group")
                        .WithMany("Configurations")
                        .HasForeignKey("FeeConfigurationGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Returns.Domain.Entities.ReturnFee", b =>
                {
                    b.HasOne("Returns.Domain.Entities.FeeConfiguration", "Configuration")
                        .WithMany("Fees")
                        .HasForeignKey("FeeConfigurationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Returns.Domain.Entities.Return", "Return")
                        .WithMany("Fees")
                        .HasForeignKey("ReturnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Returns.Domain.Entities.ReturnLine", "Line")
                        .WithMany("Fees")
                        .HasForeignKey("ReturnLineId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Configuration");

                    b.Navigation("Line");

                    b.Navigation("Return");
                });

            modelBuilder.Entity("Returns.Domain.Entities.ReturnLine", b =>
                {
                    b.HasOne("Returns.Domain.Entities.Return", "Return")
                        .WithMany("Lines")
                        .HasForeignKey("ReturnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Return");
                });

            modelBuilder.Entity("Returns.Domain.Entities.ReturnLineAttachment", b =>
                {
                    b.HasOne("Returns.Domain.Entities.ReturnLine", "Line")
                        .WithMany("Attachments")
                        .HasForeignKey("ReturnLineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Line");
                });

            modelBuilder.Entity("Returns.Domain.Entities.FeeConfiguration", b =>
                {
                    b.Navigation("Fees");
                });

            modelBuilder.Entity("Returns.Domain.Entities.FeeConfigurationGroup", b =>
                {
                    b.Navigation("Configurations");
                });

            modelBuilder.Entity("Returns.Domain.Entities.Return", b =>
                {
                    b.Navigation("Fees");

                    b.Navigation("Lines");
                });

            modelBuilder.Entity("Returns.Domain.Entities.ReturnLine", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Fees");
                });
#pragma warning restore 612, 618
        }
    }
}
