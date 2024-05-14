namespace Gamex.Models;
public class RoundMatch : Entity
{
    public string Name { get; set; } = default!;
    public Guid TournamentRoundId { get; set; }
    public TournamentRound TournamentRound { get; set; } = default!;
    public ICollection<MatchUser> MatchUser { get; set; } = default!;
}
