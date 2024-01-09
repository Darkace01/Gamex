using Microsoft.AspNetCore.Http;

namespace Gamex.DTO;

public class PostDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool? IsArchived { get; set; } = false;
    public string PictureUrl { get; set; } = string.Empty;
    public string PicturePublicId { get; set; } = string.Empty;
    public UserProfileDTO User { get; set; } = new();
    public IEnumerable<CommentDTO> Comment { get; set; }

    public PostDTO()
    {
    }

    public PostDTO(Guid id, string title, string content, bool? isArchived, string pictureUrl, string picturePublicId, UserProfileDTO user, IEnumerable<CommentDTO> comment)
    {
        Id = id;
        Title = title;
        Content = content;
        IsArchived = isArchived;
        PictureUrl = pictureUrl;
        PicturePublicId = picturePublicId;
        User = user;
        Comment = comment;
    }
}

public class PostCreateDTO
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool? IsArchived { get; set; } = false;
    public Guid? PictureId { get; set; }
    public IFormFile Picture { get; set; }
    public string UserId { get; set; }

    public PostCreateDTO()
    {
    }

    public PostCreateDTO(string title, string content, bool? isArchived, Guid? pictureId, IFormFile picture, string userId)
    {
        Title = title;
        Content = content;
        IsArchived = isArchived;
        PictureId = pictureId;
        Picture = picture;
        UserId = userId;
    }
}

public class PostUpdateDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool? IsArchived { get; set; } = false;
    public Guid? PictureId { get; set; }
    public IFormFile Picture { get; set; }
    public string UserId { get; set; }

    public PostUpdateDTO()
    {
    }

    public PostUpdateDTO(Guid id, string title, string content, bool? isArchived, Guid? pictureId, IFormFile picture, string userId)
    {
        Id = id;
        Title = title;
        Content = content;
        IsArchived = isArchived;
        PictureId = pictureId;
        Picture = picture;
        UserId = userId;
    }
}