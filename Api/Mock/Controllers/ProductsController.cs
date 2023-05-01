using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Returns.Domain.Mock;
using Returns.Logic.Mock.Repositories;

namespace Returns.Api.Mock.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("/mock/products")]
public class ProductsController : ControllerBase
{
    private readonly MockDbContext _dbContext;

    public ProductsController(MockDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public async Task<IActionResult> Get(string? search = null)
    {
        return Ok(
            await _dbContext
                .Set<Product>()
                .Where(p =>
                    string.IsNullOrEmpty(search) ||
                    p.Id.Contains(search) ||
                    p.Name.Contains(search)
                )
                .Select(p => new { p.Id, p.Name })
                .ToListAsync()
        );
    }
}
