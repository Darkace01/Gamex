namespace Gamex.DTO;

public class LeaderboardDTO
{
    public int Rank { get; set; }
    public string PlayerName { get; set; }
    public string PlayerProfilePictureUrl { get; set; }
    public string PlayerId { get; set; }
    public int Tournaments { get; set; }
    public int Points { get; set; }
}
