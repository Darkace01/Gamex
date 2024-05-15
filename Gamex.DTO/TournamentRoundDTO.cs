namespace Gamex.DTO;
public class TournamentRoundDTO
{
    public Guid Id { get; set; } = Guid.Empty!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public TournamentMiniDTO? Tournament { get; set; }
    public IEnumerable<MatchDTO> Matches { get; set; }

    public int MatchesCount { get; set; }

    public TournamentRoundDTO()
    {

    }
    public TournamentRoundDTO(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public TournamentRoundDTO(Guid id, string name, string description, TournamentMiniDTO tournament)
    {
        Id = id;
        Name = name;
        Description = description;
        Tournament = tournament;
    }
    public TournamentRoundDTO(Guid id, string name, string description, TournamentMiniDTO tournament, IEnumerable<MatchDTO> matches)
    {
        Id = id;
        Name = name;
        Description = description;
        Tournament = tournament;
        Matches = matches;
    }
    public TournamentRoundDTO(Guid id, string name, string description, TournamentMiniDTO tournament, int matchCount)
    {
        Id = id;
        Name = name;
        Description = description;
        Tournament = tournament;
        MatchesCount = matchCount;
    }
}

public class TournamentRoundCreateDTO
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Guid TournamentId { get; set; }
}

public class TournamentRoundUpdateDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Guid TournamentId { get; set; }
}
