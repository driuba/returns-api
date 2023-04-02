using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Entities;

namespace Returns.Logic.Repositories.Configurations;

public class ReturnFeeConfiguration : EntityTrackableConfiguration<ReturnFee>
{
    public ReturnFeeConfiguration(Expression<Func<ReturnFee, bool>>? queryFilterExpression)
        : base(queryFilterExpression)
    {
    }

    public override void Configure(EntityTypeBuilder<ReturnFee> builder)
    {
        builder.ToTable("ReturnFees");

        builder.HasKey(rf => rf.Id);

        builder
            .Property(rf => rf.FeeConfigurationId)
            .IsRequired();

        builder
            .Property(rf => rf.Id)
            .IsRequired();

        builder
            .Property(rf => rf.ProductId)
            .HasMaxLength(20)
            .IsRequired(false);

        builder
            .Property(rf => rf.ReturnId)
            .IsRequired();

        builder
            .Property(rf => rf.ReturnLineId)
            .IsRequired(false);

        builder
            .Property(rf => rf.Value)
            .IsRequired(false);

        builder
            .HasOne(rf => rf.Configuration)
            .WithMany(f => f.Fees)
            .HasForeignKey(rf => rf.FeeConfigurationId)
            .HasPrincipalKey(f => f.Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(rf => rf.Line)
            .WithMany(rl => rl.Fees)
            .HasForeignKey(rf => rf.ReturnLineId)
            .HasPrincipalKey(rl => rl.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(rf => rf.Return)
            .WithMany(r => r.Fees)
            .HasForeignKey(rf => rf.ReturnId)
            .HasPrincipalKey(r => r.Id)
            .OnDelete(DeleteBehavior.Cascade);

        base.Configure(builder);
    }
}
