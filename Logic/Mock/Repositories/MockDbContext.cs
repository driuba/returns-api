using Microsoft.EntityFrameworkCore;
using Returns.Logic.Mock.Repositories.Configurations;
using Returns.Logic.Repositories;

namespace Returns.Logic.Mock.Repositories;

public sealed class MockDbContext : DbContext
{
    public MockDbContext(DbContextOptions<MockDbContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(18, 4);

        configurationBuilder
            .Properties<string>()
            .UseCollation("NOCASE");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        optionsBuilder.AddInterceptors(new SqliteConnectionInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new CompanyConfiguration())
            .ApplyConfiguration(new CompanyCustomerConfiguration())
            .ApplyConfiguration(new CustomerConfiguration())
            .ApplyConfiguration(new InvoiceConfiguration())
            .ApplyConfiguration(new InvoiceLineConfiguration())
            .ApplyConfiguration(new ProductConfiguration())
            .ApplyConfiguration(new RegionConfiguration());
    }
}
