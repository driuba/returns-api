using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Mock;

namespace Returns.Logic.Mock.Repositories.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => new
        {
            c.CompanyId,
            c.Id
        });

        builder
            .Property(c => c.CompanyId)
            .IsRequired()
            .HasMaxLength(3);

        builder
            .Property(c => c.CountryId)
            .IsRequired();

        builder
            .Property(c => c.Id)
            .IsRequired()
            .HasMaxLength(20);

        builder
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(c => c.ParentId)
            .IsRequired(false)
            .HasMaxLength(20);

        builder
            .HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .HasPrincipalKey(c => c.Id);

        builder
            .HasOne(c => c.Country)
            .WithMany(r => r.Customers)
            .HasForeignKey(c => c.CountryId)
            .HasPrincipalKey(r => r.Id);
    }
}
