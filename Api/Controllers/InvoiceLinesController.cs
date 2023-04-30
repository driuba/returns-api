using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Returns.Domain.Api;
using Returns.Domain.Services;

namespace Returns.Api.Controllers;

public class InvoiceLinesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IReturnService _returnService;

    public InvoiceLinesController(IMapper mapper, IReturnService returnService)
    {
        _mapper = mapper;
        _returnService = returnService;
    }

    [HttpPost("filterInvoiceLines")]
    public async Task<IActionResult> Filter(string companyId, FilterInvoiceLinesRequest request)
    {
        return Ok(
            _mapper.ProjectTo<InvoiceLineReturnable>(
                await _returnService.FilterInvoiceLinesReturnableAsync(
                    request.DeliveryPointId,
                    request.From,
                    request.ProductId,
                    request.Search,
                    request.Skip,
                    request.To,
                    request.Top
                )
            )
        );
    }
}
