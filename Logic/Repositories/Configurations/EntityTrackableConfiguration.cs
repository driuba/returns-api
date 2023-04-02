using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Entities;

namespace Returns.Logic.Repositories.Configurations;

public abstract class EntityTrackableConfiguration<T> : EntityTypeConfiguration<T> where T : EntityTrackable
{
    protected EntityTrackableConfiguration(Expression<Func<T, bool>>? queryFilterExpression)
        : base(queryFilterExpression)
    {
    }

    public override void Configure(EntityTypeBuilder<T> builder)
    {
        builder
            .Property(et => et.Created)
            .IsRequired();

        builder
            .Property(et => et.Modified)
            .IsRequired(false);

        builder
            .Property(et => et.UserCreated)
            .HasMaxLength(30)
            .IsRequired();

        builder
            .Property(et => et.UserModified)
            .HasMaxLength(30)
            .IsRequired(false);

        base.Configure(builder);
    }
}
