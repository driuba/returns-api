using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Returns.Domain.Entities;
using Returns.Logic.Repositories.Configurations;

namespace Returns.Logic.Repositories;

public sealed class ReturnDbContext : DbContext
{
    private readonly IPrincipal _principal;

    public ReturnDbContext(DbContextOptions options, IPrincipal principal) : base(options)
    {
        _principal = principal;

        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.StateChanged += UpdatePropertiesTracking;
        ChangeTracker.Tracked += UpdatePropertiesTracking;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(18, 4);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
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

    private void UpdatePropertiesTracking(object? _, EntityEntryEventArgs eventArgs)
    {
        if (eventArgs.Entry.Entity is not EntityTrackable entity)
        {
            return;
        }

        if (eventArgs.Entry.State == EntityState.Added)
        {
            entity.Modified = DateTime.Now;
            entity.UserModified = _principal.Identity?.Name ?? "Anonymous";
        }
        else if (eventArgs.Entry.State == EntityState.Modified)
        {
            entity.Created = DateTime.Now;
            entity.UserCreated = _principal.Identity?.Name ?? "Anonymous";
        }
    }
}
