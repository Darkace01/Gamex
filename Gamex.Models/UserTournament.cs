﻿namespace Gamex.Models;

public class UserTournament
{
    public string UserId { get; set; } = string.Empty;
    public string CreatorId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public Guid TournamentId { get; set; }
    public Tournament Tournament { get; set; } = null!;
    public DateTime? DateJoined { get; set; } = DateTime.UtcNow;
    public decimal? Amount { get; set; }
    public Guid? PaymentTransactionId { get; set; }
    public PaymentTransaction? PaymentTransaction { get; set; }
}
