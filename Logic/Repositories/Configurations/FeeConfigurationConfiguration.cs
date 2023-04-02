using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Entities;

namespace Returns.Logic.Repositories.Configurations;

public class FeeConfigurationConfiguration : EntityTrackableConfiguration<FeeConfiguration>
{
    public FeeConfigurationConfiguration(Expression<Func<FeeConfiguration, bool>>? queryFilterExpression)
        : base(queryFilterExpression)
    {
    }

    public override void Configure(EntityTypeBuilder<FeeConfiguration> builder)
    {
        builder.ToTable(
            "FeeConfigurations",
            b =>
            {
                b.HasCheckConstraint(
                    "CK_FeeConfigurations_CountryId_CustomerId",
                    $"[{nameof(FeeConfiguration.CountryId)}] IS NULL OR [{nameof(FeeConfiguration.CustomerId)}] IS NULL"
                );
            }
        );

        builder.HasKey(fc => fc.Id);

        builder
            .Property(fc => fc.CountryId)
            .HasMaxLength(2)
            .IsRequired(false);

        builder
            .Property(fc => fc.CustomerId)
            .HasMaxLength(20)
            .IsRequired(false);

        builder
            .Property(fc => fc.Deleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder
            .Property(fc => fc.FeeConfigurationGroupId)
            .IsRequired();

        builder
            .Property(fc => fc.Id)
            .IsRequired();

        builder
            .Property(fc => fc.Value)
            .IsRequired();

        builder
            .Property(fc => fc.ValueMinimum)
            .IsRequired(false);

        builder
            .Property(fc => fc.ValueType)
            .IsRequired();

        builder
            .HasOne(fc => fc.Group)
            .WithMany(fcg => fcg.Configurations)
            .HasForeignKey(fc => fc.FeeConfigurationGroupId)
            .HasPrincipalKey(fcg => fcg.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(fc => new { fc.CountryId, fc.CustomerId, fc.FeeConfigurationGroupId })
            .IsUnique()
            .HasFilter($"[{nameof(FeeConfiguration.Deleted)}] = 0");

        base.Configure(builder);
    }
}
