﻿using JetBrains.Annotations;

namespace Returns.Domain.Entities;

[UsedImplicitly]
public class ReturnLineDevice : EntityTrackable
{
    public ReturnLineDevice(string serialNumber)
    {
        SerialNumber = serialNumber;
    }

    public int Id { get; set; }

    public virtual ReturnLine Line { get; set; } = default!;

    public int ReturnLineId { get; set; }

    public string SerialNumber { get; set; }
}
