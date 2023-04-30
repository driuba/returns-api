namespace Returns.Domain.Api;

public class FilterInvoiceLinesRequest
{
    public string DeliveryPointId { get; init; } = default!;

    public DateTime? From { get; init; }

    public string? ProductId { get; init; }

    public string? Search { get; init; }

    public int? Skip { get; init; }

    public DateTime? To { get; init; }

    public int? Top { get; init; }
}
