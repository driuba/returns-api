using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Returns.Api.Documentation;
using Returns.Api.Mappings;
using Returns.Api.Utils;
using Returns.Domain.Services;
using Returns.Logic.Services;
using Returns.Logic.Utils;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(b =>
    {
        b
            .WithHeaders(
                HeaderNames.Authorization,
                HeaderNames.ContentType
            )
            .WithMethods(
                HttpMethods.Delete,
                HttpMethods.Get,
                HttpMethods.Options,
                HttpMethods.Patch,
                HttpMethods.Post,
                HttpMethods.Put
            )
            .WithOrigins(
                builder.Configuration
                    .GetSection("CorsOrigins")
                    .Get<string[]>() ?? Array.Empty<string>()
            );
    });
});

builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        o.JsonSerializerOptions.PropertyNamingPolicy = null;
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .AddOData(o =>
    {
        var conventions = o.Conventions
            .Where(c => c is not AttributeRoutingConvention)
            .ToList();

        foreach (var convention in conventions)
        {
            o.Conventions.Remove(convention);
        }

        o.EnableNoDollarQueryOptions = false;
        o.RouteOptions.EnableActionNameCaseInsensitive = true;
        o.RouteOptions.EnableControllerNameCaseInsensitive = true;
        o.RouteOptions.EnableKeyAsSegment = false;
        o.RouteOptions.EnablePropertyNameCaseInsensitive = true;

        o.AddEdm();

        o.EnableQueryFeatures();
    })
    .ConfigureApiBehaviorOptions(o =>
    {
        o.InvalidModelStateResponseFactory = InvalidModelStateHandler.ResponseFactory;
        o.SuppressMapClientErrors = true;
    });

builder.Services.AddAutoMapper(e =>
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

    e.AddProfile<Profile>();
});

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = "mock";

    o.AddScheme<MockAuthenticationHandler>("mock", "mock");
});

builder.Services.AddAuthorization(o =>
{
    o.DefaultPolicy = o.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddServices(
    builder.Environment,
    builder.Configuration,
    BuildSessionService
);

if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
{
    builder.Services.AddSwaggerGen(o =>
    {
        o.CustomSchemaIds(t => t.ToString());

        o.IncludeXmlComments(
            Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"),
            includeControllerXmlComments: true
        );

        o.SupportNonNullableReferenceTypes();

        o.UseAllOfToExtendReferenceSchemas();

        o.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Title = "Returns API v1",
                Version = "v1"
            }
        );

        o.OperationFilter<InvalidModelStateFilter>();
        o.OperationFilter<ODataDeltaFilter>();
        o.OperationFilter<ODataQueryOptionsFilter>();
        o.OperationFilter<UnhandledExceptionFilter>();
        o.OperationFilter<UnresolvedReferenceFilter>();

        o.SchemaFilter<ComponentModelFilter>();

        o.MapType(
            typeof(ODataQueryOptions<>),
            () => new OpenApiSchema
            {
                UnresolvedReference = true
            }
        );

        o.MapType(
            typeof(Delta<>),
            () => new OpenApiSchema
            {
                UnresolvedReference = true
            }
        );
    });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else if (app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseExceptionHandler(b => b.Run(ExceptionHandler.HandleExceptionUnhandledAsync));
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();

    app.UseSwaggerUI(o =>
    {
        o.DocExpansion(DocExpansion.None);

        o.SwaggerEndpoint("/swagger/v1/swagger.json", "Returns API v1");
    });
}

if (app.Environment.IsDevelopment())
{
    app.UseODataRouteDebug();
}

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

static ISessionService BuildSessionService(IServiceProvider serviceProvider)
{
    var context = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;

    if (context is null)
    {
        throw new InvalidOperationException("HTTP context is required.");
    }

    var companyId = (string?)context.GetRouteData().Values["companyId"];

    if (string.IsNullOrEmpty(companyId))
    {
        throw new InvalidOperationException("Company identifier is required.");
    }

    return new SessionService(companyId, context.User);
}
