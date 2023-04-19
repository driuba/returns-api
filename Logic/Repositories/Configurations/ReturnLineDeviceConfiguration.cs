using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Entities;

namespace Returns.Logic.Repositories.Configurations;

public class ReturnLineDeviceConfiguration : EntityTrackableConfiguration<ReturnLineDevice>
{
    public ReturnLineDeviceConfiguration(Expression<Func<ReturnLineDevice, bool>>? queryFilterExpression) : base(queryFilterExpression)
    {
    }

    public override void Configure(EntityTypeBuilder<ReturnLineDevice> builder)
    {
        builder.ToTable("ReturnLineDevices");

        builder.HasKey(rld => rld.Id);

        builder
            .Property(rld => rld.Id)
            .IsRequired();

        builder
            .Property(rld => rld.ReturnLineId)
            .IsRequired();

        builder
            .Property(rld => rld.SerialNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .HasOne(rld => rld.Line)
            .WithMany(rl => rl.Devices)
            .HasForeignKey(rld => rld.ReturnLineId)
            .HasPrincipalKey(rl => rl.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);


        builder
            .HasIndex(rld => rld.ReturnLineId)
            .IsUnique();

        builder.HasIndex(rld => rld.SerialNumber);

        base.Configure(builder);
    }
}
