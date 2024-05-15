namespace Gamex.DTO;
public class MatchUserDTO
{
    public Guid MatchUserId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string PictureUrl { get; set; } = string.Empty;
    public int Points { get; set; }
    public int Rank { get; set; }
    public bool Win { get; set; }
    public bool Loss { get; set; }
    public bool Draw { get; set; }
    public Guid MatchId { get; set; }

    public MatchUserDTO()
    {

    }

    public MatchUserDTO(Guid id, string userId, string email, string displayName, string pictureUrl, int points, int rank, bool win, bool loss, bool draw, Guid matchId)
    {
        MatchUserId = id;
        UserId = userId;
        Email = email;
        DisplayName = displayName;
        PictureUrl = pictureUrl;
        Points = points;
        Rank = rank;
        Win = win;
        Loss = loss;
        Draw = draw;
        MatchId = matchId;
    }

    public MatchUserDTO(Guid id, string userId, string email, string displayName, string pictureUrl, int points, int rank, bool win, bool loss, bool draw)
    {
        MatchUserId = id;
        UserId = userId;
        Email = email;
        DisplayName = displayName;
        PictureUrl = pictureUrl;
        Points = points;
        Rank = rank;
        Win = win;
        Loss = loss;
        Draw = draw;
    }
}


public class MatchUserCreateDTO
{
    public string UserId { get; set; } = string.Empty;
    public int Points { get; set; }
    public int Rank { get; set; }
    public bool Win { get; set; }
    public bool Loss { get; set; }
    public bool Draw { get; set; }
    public Guid MatchId { get; set; }
}

public class MatchUserUpdateDTO
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int Points { get; set; }
    public int Rank { get; set; }
    public bool Win { get; set; }
    public bool Loss { get; set; }
    public bool Draw { get; set; }
    public Guid MatchId { get; set; }
}