using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Entities;

namespace Returns.Logic.Repositories.Configurations;

public class ReturnLineAttachmentConfiguration : EntityTrackableConfiguration<ReturnLineAttachment>
{
    public ReturnLineAttachmentConfiguration(Expression<Func<ReturnLineAttachment, bool>>? queryFilterExpression) : base(queryFilterExpression)
    {
    }

    public override void Configure(EntityTypeBuilder<ReturnLineAttachment> builder)
    {
        builder.ToTable("ReturnLineAttachments");

        builder.HasKey(rla => rla.Id);

        builder
            .Property(rla => rla.Id)
            .IsRequired();

        builder
            .Property(rla => rla.Name)
            .IsRequired()
            .HasMaxLength(500);

        builder
            .Property(rla => rla.ReturnLineId)
            .IsRequired();

        builder
            .Property(rla => rla.StorageId)
            .IsRequired(false);

        builder
            .HasOne(rla => rla.Line)
            .WithMany(rl => rl.Attachments)
            .HasForeignKey(rla => rla.ReturnLineId)
            .HasPrincipalKey(rl => rl.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        base.Configure(builder);
    }
}
