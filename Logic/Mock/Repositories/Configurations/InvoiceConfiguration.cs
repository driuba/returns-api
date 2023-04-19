using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Mock;

namespace Returns.Logic.Mock.Repositories.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(i => i.Id);

        builder
            .Property(i => i.CompanyId)
            .IsRequired()
            .HasMaxLength(3);

        builder
            .Property(i => i.Created)
            .IsRequired();

        builder
            .Property(i => i.CustomerId)
            .IsRequired()
            .HasMaxLength(20);

        builder
            .Property(i => i.Id)
            .IsRequired();

        builder
            .Property(i => i.Number)
            .IsRequired()
            .HasMaxLength(20);

        builder
            .HasOne(i => i.Company)
            .WithMany(c => c.Invoices)
            .HasForeignKey(i => i.CompanyId)
            .HasPrincipalKey(c => c.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(i => i.Customer)
            .WithMany(c => c.Invoices)
            .HasForeignKey(i => i.CustomerId)
            .HasPrincipalKey(c => c.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(i => new
            {
                i.CompanyId,
                i.Number
            })
            .IsUnique();
    }
}
