namespace Gamex.DTO;
public class TournamentRoundDTO
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public TournamentMiniDTO Tournament { get; set; }
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
