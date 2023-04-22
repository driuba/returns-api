using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Entities;

namespace Returns.Logic.Repositories.Configurations;

public class ReturnConfiguration : EntityTrackableConfiguration<Return>
{
    public ReturnConfiguration(Expression<Func<Return, bool>>? queryFilterExpression) : base(queryFilterExpression)
    {
    }

    public override void Configure(EntityTypeBuilder<Return> builder)
    {
        builder.ToTable("Returns");

        builder.HasKey(r => r.Id);

        builder
            .Property(r => r.CompanyId)
            .IsRequired()
            .HasMaxLength(3);

        builder
            .Property(r => r.CustomerId)
            .IsRequired()
            .HasMaxLength(20);

        builder
            .Property(r => r.DeliveryPointId)
            .IsRequired()
            .HasMaxLength(20);

        builder
            .Property(r => r.Id)
            .IsRequired();

        builder
            .Property(r => r.LabelCount)
            .IsRequired();

        builder
            .Property(r => r.Number)
            .IsRequired()
            .HasMaxLength(20);

        builder
            .Property(r => r.State)
            .IsRequired();

        builder.HasIndex(r => new { r.CompanyId, r.CustomerId });

        builder
            .HasIndex(r => new { r.CompanyId, r.Number })
            .IsUnique();

        base.Configure(builder);
    }
}
