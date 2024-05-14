namespace Gamex.Models;

public class Picture : Entity
{
    public string Name { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string PublicId { get; set; } = string.Empty;
}
