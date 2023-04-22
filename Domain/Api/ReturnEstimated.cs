namespace Returns.Domain.Api;

public class ReturnEstimated
{
    public IEnumerable<ReturnFeeEstimated> Fees { get; init; } = Enumerable.Empty<ReturnFeeEstimated>();

    public IEnumerable<ReturnLineEstimated> Lines { get; init; } = Enumerable.Empty<ReturnLineEstimated>();

    public IEnumerable<string> Messages { get; init; } = Enumerable.Empty<string>();
}
