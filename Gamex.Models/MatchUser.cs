namespace Gamex.Models;
public class MatchUser:Entity
{
    public int? Point { get; set; }
    public bool? Win { get; set; } = false;
    public bool? Loss { get; set; } = false;
    public bool? Draw { get; set; } = false;
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public Guid MatchId { get; set; }
    public RoundMatch Match { get; set; } = default!;
}
