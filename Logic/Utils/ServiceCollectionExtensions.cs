using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Returns.Logic.Repositories;

namespace Returns.Logic.Utils;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(
        this IServiceCollection serviceCollection,
        IHostEnvironment hostEnvironment,
        IConfiguration configuration
    )
    {
        serviceCollection.AddDbContext<ReturnDbContext>(b =>
        {
            var connectionString = configuration.GetConnectionString("Return");

            if (String.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Return connection string is required.");
            }

            b.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            b.UseSqlite(
                Environment.ExpandEnvironmentVariables(connectionString),
                o =>
                {
                    o.CommandTimeout(600);

                    o.UseRelationalNulls();
                }
            );

            if (hostEnvironment.IsDevelopment())
            {
                b.EnableSensitiveDataLogging();
            }
        });

        serviceCollection.AddLogging();

        return serviceCollection;
    }
}
