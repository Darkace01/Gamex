using Gamex.Common;

namespace Gamex.DTO;

public class PaymentTransactionDTO
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid? TournamentId { get; set; }
    public decimal? Amount { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public string TransactionReference { get; set; } = "";
}
public class PaymentTransactionCreateDTO
{
    public string UserId { get; set; } = string.Empty;
    public Guid? TournamentId { get; set; }
    public decimal? Amount { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public string TransactionReference { get; set; } = "";
}