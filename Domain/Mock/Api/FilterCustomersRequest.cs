using System.ComponentModel.DataAnnotations;

namespace Returns.Domain.Mock.Api;

public class FilterCustomersRequest
{
    [Required]
    [MinLength(1)]
    public IEnumerable<string> CustomerIds { get; init; } = Enumerable.Empty<string>();
}
