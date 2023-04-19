using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Mock;

namespace Returns.Logic.Mock.Repositories.Configurations;

public class InvoiceLineConfiguration : IEntityTypeConfiguration<InvoiceLine>
{
    public void Configure(EntityTypeBuilder<InvoiceLine> builder)
    {
        builder.ToTable("InvoiceLines");

        builder.HasKey(il => il.Id);

        builder
            .Property(il => il.Id)
            .IsRequired();

        builder
            .Property(il => il.InvoiceId)
            .IsRequired();

        builder
            .Property(il => il.PriceUnit)
            .IsRequired();

        builder
            .Property(il => il.ProductId)
            .IsRequired()
            .HasMaxLength(10);

        builder
            .Property(il => il.Quantity)
            .IsRequired();

        builder
            .Property(il => il.SerialNumber)
            .IsRequired(false)
            .HasMaxLength(50);

        builder
            .HasOne(il => il.Invoice)
            .WithMany(i => i.Lines)
            .HasForeignKey(il => il.InvoiceId)
            .HasPrincipalKey(i => i.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(il => il.Product)
            .WithMany(p => p.InvoiceLines)
            .HasForeignKey(il => il.ProductId)
            .HasPrincipalKey(p => p.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
