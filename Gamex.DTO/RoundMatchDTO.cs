namespace Gamex.DTO;
public class RoundMatchDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid RoundId { get; set; }
    public TournamentRoundDTO Round { get; set; }

    public RoundMatchDTO()
    {
        
    }

    public RoundMatchDTO(Guid id, string name, Guid roundId)
    {
        Id = id;
        Name = name;
        RoundId = roundId;
    }
    
    public RoundMatchDTO(Guid id, string name, Guid roundId, TournamentRoundDTO round)
    {
        Id = id;
        Name = name;
        RoundId = roundId;
        Round = round;
    }
}

public class RoundMatchCreateDTO
{
    public string Name { get; set; } = default!;
    public Guid RoundId { get; set; }
}

public class RoundMatchUpdateDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}