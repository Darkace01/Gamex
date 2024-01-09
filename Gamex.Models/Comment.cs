namespace Gamex.Models;

public class Comment:Entity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool? IsArchived { get; set; } = false;
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public Guid PostId { get; set; }
    public Post Post { get; set; }
}
