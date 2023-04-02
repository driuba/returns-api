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
            .HasMaxLength(3)
            .IsRequired();

        builder
            .Property(fcg => fcg.DelayDays)
            .IsRequired(false);

        builder
            .Property(fcg => fcg.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(fcg => fcg.Id)
            .IsRequired();

        builder
            .Property(fcg => fcg.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(fcg => fcg.Order)
            .IsRequired();

        builder
            .Property(fcg => fcg.Type)
            .IsRequired();

        base.Configure(builder);
    }
}
