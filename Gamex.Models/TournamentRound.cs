namespace Gamex.Models;
public class TournamentRound: Entity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Guid TournamentId { get; set; }
    public Tournament Tournament { get; set; } = default!;
    public ICollection<RoundMatch> RoundMatches { get; set; } = default!;
}
