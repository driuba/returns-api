using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Returns.Domain.Services;
using Returns.Logic.Mappings;
using Returns.Logic.Mock.Repositories;
using Returns.Logic.Repositories;
using Returns.Logic.Services;

namespace Returns.Logic.Utils;

public static class ServiceCollectionExtensions
{
    public static void AddServices(
        this IServiceCollection serviceCollection,
        IHostEnvironment hostEnvironment,
        IConfiguration configuration,
        Func<IServiceProvider, ISessionService> sessionServiceFactory
    )
    {
        serviceCollection.AddLogging();

        serviceCollection.AddAutoMapper(e =>
        {
            e.AllowNullCollections = true;

            e
                .Internal()
                .ForAllPropertyMaps(
                    m =>
                        m.SourceMember is not null &&
                        m.SourceMember
                            .GetCustomAttributes(inherit: true)
                            .OfType<ReadOnlyAttribute>()
                            .Any(a => a.IsReadOnly),
                    (_, mce) => mce.Ignore()
                );

            e.AddProfile<ApiProfile>();
            e.AddProfile<LogicProfile>();
        });

        serviceCollection.AddSingleton(BuildJsonSerializerOptions);

        serviceCollection.AddDbContext<MockDbContext>(b =>
        {
            var connectionString = configuration.GetConnectionString("Mock");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Mock connection string is required.");
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

        serviceCollection.AddScoped<ICustomerService, CustomerService>();
        serviceCollection.AddScoped<IFeeConfigurationService, FeeConfigurationService>();
        serviceCollection.AddScoped<IRegionService, RegionService>();
        serviceCollection.AddScoped<IReturnService, ReturnService>();
        serviceCollection.AddScoped<IStorageService, StorageService>();
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
