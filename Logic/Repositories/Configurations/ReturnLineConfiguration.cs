using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Entities;

namespace Returns.Logic.Repositories.Configurations;

public class ReturnLineConfiguration : EntityTrackableConfiguration<ReturnLine>
{
    public override void Configure(EntityTypeBuilder<ReturnLine> builder)
    {
        builder.ToTable("ReturnLines");

        builder.HasKey(rl => rl.Id);

        builder
            .Property(rl => rl.Id)
            .IsRequired();

        builder
            .Property(rl => rl.InvoiceNumberPurchase)
            .HasMaxLength(20).IsRequired(false);

        builder
            .Property(rl => rl.InvoiceNumberReturn)
            .HasMaxLength(20).IsRequired(false);

        builder
            .Property(rl => rl.NoteReturn)
            .HasMaxLength(500)
            .IsRequired(false);

        builder
            .Property(rl => rl.NoteResponse)
            .HasMaxLength(500)
            .IsRequired(false);

        builder
            .Property(rl => rl.PriceUnit)
            .IsRequired();

        builder
            .Property(rl => rl.ProductId)
            .HasMaxLength(20)
            .IsRequired();

        builder
            .Property(rl => rl.ProductType)
            .IsRequired();

        builder
            .Property(rl => rl.Quantity)
            .IsRequired();

        builder
            .Property(rl => rl.ReturnId)
            .IsRequired();

        builder
            .Property(rl => rl.State)
            .IsRequired();

        builder
            .HasOne(rl => rl.Return)
            .WithMany(r => r.Lines)
            .HasForeignKey(rl => rl.ReturnId)
            .HasPrincipalKey(r => r.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(rl => new { rl.InvoiceNumberPurchase, rl.ProductId });

        base.Configure(builder);
    }
}
