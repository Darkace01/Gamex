namespace Gamex.Models;

public class UserTournament:ISoftDeletable
{
    public string UserId { get; set; } = string.Empty;
    public string CreatorId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public Guid TournamentId { get; set; }
    public Tournament Tournament { get; set; } = null!;
    public DateTime? DateJoined { get; set; } = DateTime.UtcNow;
    public decimal? Amount { get; set; }
    public int? Point { get; set; }
    public bool? Win { get; set; } = false;
    public bool? Loss { get; set; } = false;
    public bool? Draw { get; set; } = false;
    public bool? WaitList { get; set; } = false;
    public Guid? PaymentTransactionId { get; set; }
    public PaymentTransaction? PaymentTransaction { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; } = default!;
}
