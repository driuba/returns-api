using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Entities;

namespace Returns.Logic.Repositories.Configurations;

public class FeeConfigurationGroupConfiguration : EntityTypeConfiguration<FeeConfigurationGroup>
{
    public FeeConfigurationGroupConfiguration(Expression<Func<FeeConfigurationGroup, bool>>? queryFilterExpression) : base(queryFilterExpression)
    {
    }

    public override void Configure(EntityTypeBuilder<FeeConfigurationGroup> builder)
    {
        builder.ToTable("FeeConfigurationGroups");

        builder.HasKey(fcg => fcg.Id);

        builder
            .Property(fcg => fcg.CompanyId)
            .IsRequired()
            .HasMaxLength(3);

        builder
            .Property(fcg => fcg.DelayDays)
            .IsRequired(false);

        builder
            .Property(fcg => fcg.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder
            .Property(fcg => fcg.Id)
            .IsRequired();

        builder
            .Property(fcg => fcg.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(fcg => fcg.Order)
            .IsRequired();

        builder
            .Property(fcg => fcg.Type)
            .IsRequired();

        base.Configure(builder);
    }
}
