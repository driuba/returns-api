﻿namespace Returns.Domain.Enums;

public enum ReturnState
{
    New = 0,
    Registered = 10,
    Declined = 20,
    InvoicedPartially = 25,
    Invoiced = 30
}
