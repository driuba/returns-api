using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Returns.Domain.Entities;
using Returns.Domain.Services;
using Returns.Logic.Repositories.Configurations;

namespace Returns.Logic.Repositories;

public sealed class ReturnDbContext : DbContext
{
    private readonly ISessionService? _sessionService;

    public ReturnDbContext(DbContextOptions options, ISessionService? sessionService) : base(options)
    {
        _sessionService = sessionService;

        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.StateChanged += UpdatePropertiesTracking;
        ChangeTracker.Tracked += UpdatePropertiesTracking;
    }

    private string _companyId =>
        _sessionService?.CompanyId ??
        throw new InvalidOperationException("Session company identifier is required.");

    private string? _customerId => _sessionService?.CustomerId;

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
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(
                new FeeConfigurationConfiguration(fc =>
                    fc.Group.CompanyId == _companyId &&
                    (
                        string.IsNullOrEmpty(_customerId) ||
                        string.IsNullOrEmpty(fc.CustomerId) ||
                        fc.CustomerId == _customerId
                    )
                )
            )
            .ApplyConfiguration(
                new FeeConfigurationGroupConfiguration(fcg => fcg.CompanyId == _companyId)
            )
            .ApplyConfiguration(
                new ReturnConfiguration(r =>
                    r.CompanyId == _companyId &&
                    (
                        string.IsNullOrEmpty(_customerId) ||
                        r.CustomerId == _customerId
                    )
                )
            )
            .ApplyConfiguration(
                new ReturnAvailabilityConfiguration(ra => ra.CompanyId == _companyId)
            )
            .ApplyConfiguration(
                new ReturnFeeConfiguration(rf =>
                    rf.Return.CompanyId == _companyId &&
                    (
                        string.IsNullOrEmpty(_customerId) ||
                        rf.Return.CustomerId == _customerId
                    )
                )
            )
            .ApplyConfiguration(
                new ReturnLineConfiguration(rl =>
                    rl.Return.CompanyId == _companyId &&
                    (
                        string.IsNullOrEmpty(_customerId) ||
                        rl.Return.CustomerId == _customerId
                    )
                )
            )
            .ApplyConfiguration(
                new ReturnLineAttachmentConfiguration(rla =>
                    rla.Line.Return.CompanyId == _companyId &&
                    (
                        string.IsNullOrEmpty(_customerId) ||
                        rla.Line.Return.CustomerId == _customerId
                    )
                )
            );
    }

    private void UpdatePropertiesTracking(object? _, EntityEntryEventArgs eventArgs)
    {
        if (eventArgs.Entry.Entity is not EntityTrackable entity)
        {
            return;
        }

        switch (eventArgs)
        {
            case { Entry.State: EntityState.Added }:
                entity.Modified = DateTime.Now;
                entity.UserModified = _sessionService?.Principal.Identity?.Name ?? "Anonymous";

                break;
            case { Entry.State: EntityState.Modified }:
                entity.Created = DateTime.Now;
                entity.UserCreated = _sessionService?.Principal.Identity?.Name ?? "Anonymous";

                break;
        }
    }
}
