namespace Gamex.DTO;

public class LeaderboardDTO
{
    public int Rank { get; set; }
    public string PlayerName { get; set; }
    public string PlayerProfilePictureUrl { get; set; }
    public string PlayerId { get; set; }
    public int Tournaments { get; set; }
    public List<TournamentMiniDTO> TournamentList { get; set; }
    public int Points { get; set; }
    public int Win { get; set; }
    public int Loss { get; set; }
    public int Draw { get; set; }
    public IEnumerable<TournamentFormDTO>? Form { get; set; }
}

public class TournamentFormDTO
{
    public Guid MatchId { get; set; }
    public bool Win { get; set; }
    public bool Loss { get; set; }
    public bool Draw { get; set; }

    public TournamentFormDTO()
    {

    }

    public TournamentFormDTO(Guid matchId, bool win, bool loss, bool draw)
    {
        MatchId = matchId;
        Win = win;
        Loss = loss;
        Draw = draw;
    }
}