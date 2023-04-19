using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Mock;

namespace Returns.Logic.Mock.Repositories.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.ByOrderOnly)
            .IsRequired();

        builder
            .Property(p => p.Id)
            .IsRequired()
            .HasMaxLength(10);

        builder
            .Property(p => p.Serviceable)
            .IsRequired();
    }
}
