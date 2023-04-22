namespace Returns.Domain.Api;

public class ReturnFeeEstimated
{
    public int? DelayDays { get; init; }

    public int FeeConfigurationGroupId { get; init; }

    public decimal Value { get; init; }
}
