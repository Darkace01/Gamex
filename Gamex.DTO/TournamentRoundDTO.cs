namespace Gamex.DTO;
public class TournamentRoundDTO
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public TournamentMiniDTO Tournament { get; set; }
    public List<MatchDTO> Rounds { get; set; }

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
    public TournamentRoundDTO(Guid id, string name, string description, TournamentMiniDTO tournament, List<MatchDTO> rounds)
    {
        Id = id;
        Name = name;
        Description = description;
        Tournament = tournament;
        Rounds = rounds;
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
