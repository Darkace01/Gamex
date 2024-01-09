namespace Gamex.Service.Implementation;

public class CommentService : ICommentService
{
    private readonly GamexDbContext _context;

    public CommentService(GamexDbContext context)
    {
        _context = context;
    }

    public async Task<CommentDTO> GetCommentById(Guid id)
    {
        Comment? comment = await _context.Comments
            .AsNoTracking()
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (comment == null)
            return null;

        return new CommentDTO
        {
            Id = comment.Id,
            Content = comment.Content,
            Title = comment.Title,
            PostId = comment.PostId,
            User = new UserProfileDTO
            {
                DisplayName = comment.User.DisplayName,
                Email = comment.User.Email,
                PhoneNumber = comment.User.PhoneNumber,
                ProfilePicturePublicId = comment.User.Picture?.PublicId,
                ProfilePictureUrl = comment.User.Picture?.FileUrl
            }
        };
    }

    public IQueryable<CommentDTO> GetAllComments()
    {
        var comments = _context.Comments
                             .AsNoTracking()
                             .Include(c => c.User);

        return comments.Select(c => new CommentDTO
        {
            Id = c.Id,
            Content = c.Content,
            Title = c.Title,
            PostId = c.PostId,
            User = new UserProfileDTO
            {
                DisplayName = c.User.DisplayName,
                Email = c.User.Email,
                PhoneNumber = c.User.PhoneNumber,
                ProfilePicturePublicId = c.User.Picture == null ? "" : c.User.Picture.PublicId,
                ProfilePictureUrl = c.User.Picture == null ? "" : c.User.Picture.FileUrl
            }
        });
    }

    public async Task<bool> CreateComment(CommentCreateDTO commentCreateDTO, string userId)
    {
        var comment = new Comment()
        {
            Content = commentCreateDTO.Content,
            Title = commentCreateDTO.Title,
            PostId = commentCreateDTO.PostId,
            UserId = userId
        };
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateComment(CommentUpdateDTO commentUpdateDTO)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentUpdateDTO.Id);
        if (comment == null)
        {
            return false;
        }
        comment.Content = commentUpdateDTO.Content;
        comment.Title = commentUpdateDTO.Title;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteComment(Guid commentId)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
        if (comment == null)
        {
            return false;
        }
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCommentByPostId(Guid postId)
    {
        var comments = await _context.Comments.Where(c => c.PostId == postId).ToListAsync();
        if (comments == null)
        {
            return false;
        }
        _context.Comments.RemoveRange(comments);
        await _context.SaveChangesAsync();
        return true;
    }

    public IQueryable<CommentDTO> GetAllCommentByPostId(Guid postId)
    {
        var comments = _context.Comments
                             .AsNoTracking()
                             .Include(c => c.User)
                             .Where(c => c.PostId == postId);

        return comments.Select(c => new CommentDTO
        {
            Id = c.Id,
            Content = c.Content,
            Title = c.Title,
            PostId = c.PostId,
            User = new UserProfileDTO
            {
                DisplayName = c.User.DisplayName,
                Email = c.User.Email,
                PhoneNumber = c.User.PhoneNumber,
                ProfilePicturePublicId = c.User.Picture == null ? "" : c.User.Picture.PublicId,
                ProfilePictureUrl = c.User.Picture == null ? "" : c.User.Picture.FileUrl
            }
        }).AsQueryable();
    }
}
