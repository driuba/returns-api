using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.OData.Edm.Csdl;

namespace Returns.Api.Controllers;

[Produces(MediaTypeNames.Application.Xml)]
public class ODataMetadataController : ControllerBase
{
    private static readonly Version _version = new(4, 0);

    [HttpGet("$metadata")]
    public IActionResult Get(string companyId)
    {
        var model = Request.GetModel();

        model.SetEdmxVersion(_version);

        return Ok(model);
    }
}
