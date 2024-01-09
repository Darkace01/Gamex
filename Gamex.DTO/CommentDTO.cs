namespace Gamex.DTO;

public class CommentDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool? IsArchived { get; set; } = false;
    public UserProfileDTO User { get; set; } = new();
    public Guid PostId { get; set; }

    public CommentDTO()
    {
    }

    public CommentDTO(Guid id, string title, string content, bool? isArchived, UserProfileDTO user, Guid postId)
    {
        Id = id;
        Title = title;
        Content = content;
        IsArchived = isArchived;
        User = user;
        PostId = postId;
    }
}

public class CommentCreateDTO
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool? IsArchived { get; set; } = false;
    public Guid PostId { get; set; }
    public CommentCreateDTO()
    {
    }
    public CommentCreateDTO(string title, string content, bool? isArchived, Guid postId)
    {
        Title = title;
        Content = content;
        IsArchived = isArchived;
        PostId = postId;
    }
}

public class CommentUpdateDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool? IsArchived { get; set; } = false;
    public Guid PostId { get; set; }
    public CommentUpdateDTO()
    {
    }
    public CommentUpdateDTO(Guid id, string title, string content, bool? isArchived, Guid postId)
    {
        Id = id;
        Title = title;
        Content = content;
        IsArchived = isArchived;
        PostId = postId;
    }
}