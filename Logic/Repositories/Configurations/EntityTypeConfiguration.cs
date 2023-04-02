using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Entities;

namespace Returns.Logic.Repositories.Configurations;

public abstract class EntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IEntity
{
    private readonly Expression<Func<T, bool>>? _queryFilterExpression;

    protected EntityTypeConfiguration(Expression<Func<T, bool>>? queryFilterExpression)
    {
        _queryFilterExpression = queryFilterExpression;
    }

    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        if (_queryFilterExpression is not null)
        {
            builder.HasQueryFilter(_queryFilterExpression);
        }
    }
}
