namespace Gamex.DTO;

public class TournamentCategoryDTO
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TournamentCategoryCreateDTO
{
    public string Name { get; set; } = string.Empty;
}

public class TournamentCategoryUpdateDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}