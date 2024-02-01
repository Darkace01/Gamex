namespace Gamex.Models;

public class PaymentTransaction : Entity
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public Guid? TournamentId { get; set; }
    public Tournament Tournament { get; set; } = null!;
    public decimal? Amount { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public string TransactionReference { get; set; } = "";
}
