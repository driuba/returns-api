using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Returns.Domain.Mock;

namespace Returns.Logic.Mock.Repositories.Configurations;

public class CompanyCustomerConfiguration : IEntityTypeConfiguration<CompanyCustomer>
{
    public void Configure(EntityTypeBuilder<CompanyCustomer> builder)
    {
        builder.ToTable("CompanyCustomers");

        builder.HasKey(cc => new
        {
            cc.CompanyId,
            cc.CustomerId
        });

        builder
            .Property(cc => cc.CompanyId)
            .IsRequired()
            .HasMaxLength(3);

        builder
            .Property(cc => cc.CustomerId)
            .IsRequired()
            .HasMaxLength(20);

        builder
            .HasOne(cc => cc.Company)
            .WithMany(c => c.Customers)
            .HasForeignKey(cc => cc.CompanyId)
            .HasPrincipalKey(c => c.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(cc => cc.Customer)
            .WithMany(c => c.Companies)
            .HasForeignKey(cc => cc.CustomerId)
            .HasPrincipalKey(c => c.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
