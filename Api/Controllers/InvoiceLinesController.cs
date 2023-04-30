using Microsoft.AspNetCore.Mvc;
using Returns.Domain.Api;
using Returns.Domain.Services;

namespace Returns.Api.Controllers;

public class InvoiceLinesController : ControllerBase
{
    private readonly IReturnService _returnService;

    public InvoiceLinesController(IReturnService returnService)
    {
        _returnService = returnService;
    }

    [HttpPost("filterInvoiceLines")]
    public async Task<IActionResult> Filter(string companyId, [FromBody] FilterInvoiceLinesRequest request)
    {
        return Ok(
            await _returnService.FilterInvoiceLinesReturnableAsync(
                request.DeliveryPointId,
                request.From,
                request.ProductId,
                request.Search,
                request.Skip,
                request.To,
                request.Top
            )
        );
    }
}
