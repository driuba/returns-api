namespace Returns.Domain.Api;

public class ReturnEstimated
{
    public IEnumerable<ReturnFeeEstimated> Fees { get; set; } = Enumerable.Empty<ReturnFeeEstimated>();

    public IEnumerable<ReturnLineEstimated> Lines { get; set; } = Enumerable.Empty<ReturnLineEstimated>();

    public IEnumerable<string> Messages { get; set; } = Enumerable.Empty<string>();
}
