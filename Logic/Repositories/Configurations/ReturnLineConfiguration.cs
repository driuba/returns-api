using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Entities;

namespace Returns.Logic.Repositories.Configurations;

public class ReturnLineConfiguration : EntityTrackableConfiguration<ReturnLine>
{
    public ReturnLineConfiguration(Expression<Func<ReturnLine, bool>>? queryFilterExpression) : base(queryFilterExpression)
    {
    }

    public override void Configure(EntityTypeBuilder<ReturnLine> builder)
    {
        builder.ToTable("ReturnLines");

        builder.HasKey(rl => rl.Id);

        builder
            .Property(rl => rl.Id)
            .IsRequired();

        builder
            .Property(rl => rl.InvoiceNumberPurchase)
            .IsRequired(false)
            .HasMaxLength(20);

        builder
            .Property(rl => rl.InvoiceNumberReturn)
            .IsRequired(false)
            .HasMaxLength(20);

        builder
            .Property(rl => rl.NoteReturn)
            .IsRequired(false)
            .HasMaxLength(500);

        builder
            .Property(rl => rl.NoteResponse)
            .IsRequired(false)
            .HasMaxLength(500);

        builder
            .Property(rl => rl.PriceUnit)
            .IsRequired();

        builder
            .Property(rl => rl.ProductId)
            .IsRequired()
            .HasMaxLength(10);

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
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(rl => new { rl.InvoiceNumberPurchase, rl.ProductId });

        base.Configure(builder);
    }
}
