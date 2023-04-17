using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Mock;

namespace Returns.Logic.Mock.Repositories.Configurations;

public class RegionConfiguration : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder.ToTable("Regions");

        builder.HasKey(r => r.Id);

        builder
            .Property(r => r.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(r => r.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(r => r.Type)
            .IsRequired();

        builder
            .HasMany(r => r.Children)
            .WithMany(r => r.Parents);
    }
}
