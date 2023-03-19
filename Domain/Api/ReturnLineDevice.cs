using System.ComponentModel;

namespace Returns.Domain.Api;

public class ReturnLineDevice : TrackableBase
{
    public ReturnLineDevice(string serialNumber)
    {
        SerialNumber = serialNumber;
    }

    [ReadOnly(true)] public int Id { get; set; }

    [ReadOnly(true)] public virtual ReturnLine Line { get; set; } = default!;

    [ReadOnly(true)] public int ReturnLineId { get; set; }

    [ReadOnly(true)] public string SerialNumber { get; set; }
}
