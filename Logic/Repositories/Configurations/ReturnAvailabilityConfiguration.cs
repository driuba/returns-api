﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Entities;

namespace Returns.Logic.Repositories.Configurations;

public class ReturnAvailabilityConfiguration : EntityTypeConfiguration<ReturnAvailability>
{
    public ReturnAvailabilityConfiguration(Expression<Func<ReturnAvailability, bool>>? queryFilterExpression)
        : base(queryFilterExpression)
    {
    }

    public override void Configure(EntityTypeBuilder<ReturnAvailability> builder)
    {
        builder.ToTable("ReturnAvailabilities");

        builder.HasKey(ra => ra.Id);

        builder
            .Property(ra => ra.CompanyId)
            .HasMaxLength(3)
            .IsRequired();

        builder
            .Property(ra => ra.CountryId)
            .HasMaxLength(2)
            .IsRequired(false);

        builder
            .Property(ra => ra.Days)
            .IsRequired();

        builder
            .Property(ra => ra.Id)
            .IsRequired();

        builder
            .HasIndex(ra => new { ra.CompanyId, ra.CountryId })
            .IsUnique()
            .HasFilter(null);

        base.Configure(builder);
    }
}
