using Returns.Domain.Entities;

namespace Returns.Domain.Dto;

public class ReturnFee
{
    public ReturnFee(FeeConfiguration configuration)
    {
        Configuration = configuration;
    }

    public FeeConfiguration Configuration { get; set; }

    public int? DelayDays { get; set; }

    public string? ProductId { get; set; }

    public decimal Value { get; set; }
}
