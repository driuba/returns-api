using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Returns.Domain.Api;

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

        builder.EntitySet<FeeConfiguration>("feeConfigurations");
        builder.EntitySet<FeeConfigurationGroup>("feeConfigurationGroups");
        builder.EntitySet<Return>("returns");
        builder.EntitySet<ReturnAvailability>("returnAvailabilities");
        builder.EntitySet<ReturnFee>("returnFees");
        builder.EntitySet<ReturnLine>("returnLines");
        builder.EntitySet<ReturnLineAttachment>("returnLineAttachments");

        builder
            .EntityType<ReturnLineAttachment>()
            .HasKey(rla => rla.StorageId);

        var action = builder
            .EntityType<Return>().Collection
            .Action("estimate");

        action
            .Parameter<ReturnRequest>("request")
            .Required();

        action.Returns<ReturnEstimated>();

        var function = builder
            .EntityType<ReturnAvailability>().Collection
            .Function("filter");

        function
            .Parameter<string>("deliveryPointId")
            .Required();

        function.ReturnsFromEntitySet<ReturnAvailability>("returnAvailabilities");

        action = builder.Action("filterInvoiceLines");

        action
            .Parameter<FilterInvoiceLinesRequest>("request")
            .Required();

        action.ReturnsCollection<InvoiceLineReturnable>();

        return builder.GetEdmModel();
    }
}
