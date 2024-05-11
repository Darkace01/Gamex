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
    public string Prize { get; set; } = string.Empty;
    public Guid? PictureId { get; set; }
    public Picture? Picture { get; set; }
    public Guid? CoverPictureId { get; set; }
    public Picture? CoverPicture { get; set; }
    public int? AvailableSlot { get; set; } = 1000;
    public List<UserTournament> UserTournaments { get; set; } = default!;
    public List<TournamentCategory> Categories { get; set; } = default!;
    public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = default!;
    public ICollection<TournamentRound> TournamentRounds { get; set; } = default!;
}
