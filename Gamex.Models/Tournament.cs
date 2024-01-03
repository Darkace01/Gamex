namespace Gamex.Models;

public class Tournament : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsFeatured { get; set; } = false;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime? Time { get; set; }
    public decimal EntryFee { get; set; }
    public string Rules { get; set; } = string.Empty;
    public Guid? PictureId { get; set; }
    public Picture? Picture { get; set; }
    public List<UserTournament> UserTournaments { get; set; } = new();
}
