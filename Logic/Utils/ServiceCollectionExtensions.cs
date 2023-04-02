using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Returns.Domain.Services;
using Returns.Logic.Repositories;
using Returns.Logic.Services;

namespace Returns.Logic.Utils;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection serviceCollection,
        IHostEnvironment hostEnvironment,
        IConfiguration configuration,
        Func<IServiceProvider, ISessionService> sessionServiceFactory)
    {
        serviceCollection.AddLogging();

        serviceCollection.AddSingleton(BuildJsonSerializerOptions);

        serviceCollection.AddDbContext<ReturnDbContext>(b =>
        {
            var connectionString = configuration.GetConnectionString("Return");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Return connection string is required.");
            }

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

        serviceCollection.AddScoped(sessionServiceFactory);

        serviceCollection.AddScoped<IFeeConfigurationService, FeeConfigurationService>();
    }

    private static JsonSerializerOptions BuildJsonSerializerOptions(IServiceProvider _)
    {
        return new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Converters =
            {
                new JsonStringEnumConverter()
            },
            MaxDepth = 32,
            PropertyNamingPolicy = null,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
    }
}
