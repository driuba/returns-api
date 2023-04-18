using Returns.Domain.Entities;

namespace Returns.Domain.Dto;

public class ReturnFeeEstimated
{
    public ReturnFeeEstimated(FeeConfiguration configuration)
    {
        Configuration = configuration;
    }

    public FeeConfiguration Configuration { get; set; }

    public int? DelayDays { get; set; }

    public string? ProductId { get; set; }

    public decimal Value { get; set; }
}
