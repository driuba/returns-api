using Microsoft.EntityFrameworkCore;
using Returns.Logic.Repositories.Configurations;

namespace Returns.Logic.Repositories;

public class ReturnDbContext : DbContext
{
    public ReturnDbContext(DbContextOptions options) : base(options)
    {
        Init();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(18, 4);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new FeeConfigurationConfiguration())
            .ApplyConfiguration(new FeeConfigurationGroupConfiguration())
            .ApplyConfiguration(new ReturnAvailabilityConfiguration())
            .ApplyConfiguration(new ReturnConfiguration())
            .ApplyConfiguration(new ReturnFeeConfiguration())
            .ApplyConfiguration(new ReturnLineConfiguration())
            .ApplyConfiguration(new ReturnLineAttachmentConfiguration())
            .ApplyConfiguration(new ReturnLineDeviceConfiguration());
    }

    private void Init()
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }
}
