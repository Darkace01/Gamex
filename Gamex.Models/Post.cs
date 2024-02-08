namespace Gamex.Models;

public class Post:Entity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool? IsArchived { get; set; } = false;
    public Guid? PictureId { get; set; }
    public Picture? Picture { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<PostTag> PostTags { get; set; }
}
