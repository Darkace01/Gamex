namespace Gamex.Models;

public class TournamentCategory:Entity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}
