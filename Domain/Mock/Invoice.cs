using JetBrains.Annotations;

namespace Returns.Domain.Mock;

[UsedImplicitly]
public class Invoice
{
    public Invoice(string companyId, string customerId, string number)
    {
        CompanyId = companyId;
        CustomerId = customerId;
        Number = number;
    }

    public virtual Company Company { get; init; } = default!;

    public string CompanyId { get; init; }

    public DateTime Created { get; init; }

    public virtual Customer Customer { get; init; } = default!;

    public string CustomerId { get; init; }

    public int Id { get; init; }

    public virtual ICollection<InvoiceLine> Lines { get; set; } = default!;

    public string Number { get; init; }
}
