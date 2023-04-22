using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Returns.Domain.Api;
using Returns.Domain.Dto.Storage;
using Returns.Domain.Services;

namespace Returns.Api.Mock.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("mock/storage/files")]
public class StorageController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly StorageOptions _options;
    private readonly IStorageService _storageService;

    public StorageController(IMapper mapper, IOptions<StorageOptions> options, IStorageService storageService)
    {
        _mapper = mapper;
        _options = options.Value;
        _storageService = storageService;
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var response = _storageService.Delete(id);

        if (response.Success)
        {
            return NoContent();
        }

        return BadRequest(
            new Response
            {
                Message = response.Messages.FirstOrDefault()
            }
        );
    }

    [HttpGet("{id:guid}")]
    [Produces(MediaTypeNames.Application.Octet, MediaTypeNames.Application.Json)]
    public IActionResult Get(Guid id)
    {
        var response = _storageService.Get(id);

        if (response is { Success: true, Value: not null })
        {
            return File(response.Value, MediaTypeNames.Application.Octet);
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }

    [HttpPost("")]
    [Consumes(Domain.Constants.MediaTypeNames.Multipart.FormData)]
    public async Task<IActionResult> Post(IFormFile file)
    {
        await using var stream = file.OpenReadStream();

        var response = await _storageService.Create(stream);

        if (response.Success)
        {
            return Created(string.Format(_options.Url, response.Value), response.Value);
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }
}
