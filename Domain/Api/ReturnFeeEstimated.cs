namespace Returns.Domain.Api;

public class ReturnFeeEstimated
{
    public int? DelayDays { get; set; }

    public int FeeConfigurationGroupId { get; set; }

    public decimal Value { get; set; }
}
