namespace Gamex.DTO;

public class TagDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PostCount { get; set; }

    public TagDTO()
    {
    }

    public TagDTO(Guid id, string name, int postCount)
    {
        Id = id;
        Name = name;
        PostCount = postCount;
    }
}

public class TagCreateDTO
{
    public string Name { get; set; } = string.Empty;
}

public class TagUpdateDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}