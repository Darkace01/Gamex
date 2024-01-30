namespace Gamex.Models;

public class UserTournament
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public Guid TournamentId { get; set; }
    public Tournament Tournament { get; set; } = null!;
    public DateTime? DateJoined { get; set; }
    public decimal? Amount { get; set; }
}
