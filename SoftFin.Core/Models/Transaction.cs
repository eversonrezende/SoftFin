using SoftFin.Core.Enums;

namespace SoftFin.Core.Models;

public class Transaction
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidOrReceivedAt { get; set; }

    // Default value is Withdraw (saída), because it's the most common transaction
    public ETransactionType Type { get; set; } = ETransactionType.Withdraw;

    public decimal Amount { get; set; }

    public long CategoryId { get; set; }

    // Avoid using new() to initialize Category property because always create a object in the Database
    public Category Category { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
}
