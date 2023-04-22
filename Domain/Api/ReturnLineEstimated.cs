namespace Returns.Domain.Api;

public class ReturnLineEstimated : ReturnLineValidated
{
    public ReturnLineEstimated(string reference) : base(reference)
    {
    }

    public IEnumerable<ReturnFeeEstimated> Fees { get; init; } = Enumerable.Empty<ReturnFeeEstimated>();
}
