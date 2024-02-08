namespace Gamex.Models;

public class Tag : Entity
{
    public string Name { get; set; } = null!;
    public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
}
