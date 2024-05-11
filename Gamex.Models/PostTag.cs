namespace Gamex.Models;

public class PostTag: ISoftDeletable
{
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; } = default!;
}
