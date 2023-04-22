using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Returns.Api.Utils;

internal static class ODataOptionsExtensions
{
    internal static void AddEdm(this ODataOptions options)
    {
        options.AddRouteComponents("api/{companyId}", BuildEdm());
    }

    private static IEdmModel BuildEdm()
    {
        var builder = new ODataConventionModelBuilder();

        builder.EntitySet<Domain.Api.FeeConfiguration>("feeConfigurations");
        builder.EntitySet<Domain.Api.FeeConfigurationGroup>("feeConfigurationGroups");
        builder.EntitySet<Domain.Api.Return>("returns");
        builder.EntitySet<Domain.Api.ReturnAvailability>("returnAvailabilities");
        builder.EntitySet<Domain.Api.ReturnFee>("returnFees");
        builder.EntitySet<Domain.Api.ReturnLine>("returnLines");
        builder.EntitySet<Domain.Api.ReturnLineAttachment>("returnLineAttachments");

        builder
            .EntityType<Domain.Api.ReturnLineAttachment>()
            .HasKey(rla => rla.StorageId);

        return builder.GetEdmModel();
    }
}
