namespace Gamex.DTO;

public class TournamentUserDTO
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CreatorId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string PictureUrl { get; set; } = string.Empty;
    public int Points { get; set; }
    public int Rank { get; set; }
    public bool IsInWaitList { get; set; }
    public bool Win { get; set; }
    public bool Loss { get; set; }
}
public class TournamentUserUpdateDTO
{
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TournamentName { get; set; } = string.Empty;
    public Guid TournamentId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int Points { get; set; }
    public bool IsInWaitList { get; set; }
    public bool Win { get; set; }
    public bool Loss { get; set; }
}
