namespace Gamex.DTO;
public class MatchDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid RoundId { get; set; }
    public TournamentRoundDTO Round { get; set; }

    public int PlayersCount { get; set; }

    public MatchDTO()
    {

    }

    public MatchDTO(Guid id, string name, Guid roundId)
    {
        Id = id;
        Name = name;
        RoundId = roundId;
    }

    public MatchDTO(Guid id, string name, Guid roundId, TournamentRoundDTO round)
    {
        Id = id;
        Name = name;
        RoundId = roundId;
        Round = round;
    }

    public MatchDTO(Guid id, string name, Guid roundId, TournamentRoundDTO round, int playersCount)
    {
        Id = id;
        Name = name;
        RoundId = roundId;
        Round = round;
        PlayersCount = playersCount;
    }
}

public class MatchCreateDTO
{
    public string Name { get; set; } = default!;
    public Guid RoundId { get; set; }
}

public class MatchUpdateDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}