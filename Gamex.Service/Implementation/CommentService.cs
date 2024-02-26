using Gamex.Models;

namespace Gamex.Service.Implementation;

public class CommentService : ICommentService
{
    private readonly GamexDbContext _context;

    public CommentService(GamexDbContext context)
    {
        _context = context;
    }

    public async Task<CommentDTO?> GetCommentById(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAllComments().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public IQueryable<CommentDTO> GetAllComments()
    {
        var comments = _context.Comments
                     .AsNoTracking()
                     .Include(c => c.User)
                     .Join(_context.Users.AsNoTracking(),
                           comment => comment.UserId,
                           user => user.Id,
                           (comment, user) => new { Comment = comment, User = user })
                     .GroupJoin(_context.Pictures.AsNoTracking(),
                                cu => cu.User.PictureId,
                                picture => picture.Id,
                                (cu, pictures) => new { CommentUser = cu, Pictures = pictures })
                     .SelectMany(cup => cup.Pictures.DefaultIfEmpty(),
                                 (cup, picture) => new CommentDTO
                                 {
                                     Id = cup.CommentUser.Comment.Id,
                                     Content = cup.CommentUser.Comment.Content,
                                     Title = cup.CommentUser.Comment.Title,
                                     PostId = cup.CommentUser.Comment.PostId,
                                     User = new UserProfileDTO
                                     {
                                         FirstName = cup.CommentUser.User.FirstName,
                                         LastName = cup.CommentUser.User.LastName,
                                         DisplayName = cup.CommentUser.User.DisplayName,
                                         Email = cup.CommentUser.User.Email,
                                         PhoneNumber = cup.CommentUser.User.PhoneNumber,
                                         ProfilePicturePublicId = picture == null ? "" : picture.PublicId,
                                         ProfilePictureUrl = picture == null ? "" : picture.FileUrl
                                     },
                                     DateCreated = cup.CommentUser.Comment.DateCreated
                                 });
        return comments;
    }

    public async Task<bool> CreateComment(CommentCreateDTO commentCreateDTO, string userId, CancellationToken cancellationToken = default)
    {
        var comment = new Comment()
        {
            Content = commentCreateDTO.Content,
            Title = commentCreateDTO.Title,
            PostId = commentCreateDTO.PostId,
            UserId = userId
        };
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> UpdateComment(CommentUpdateDTO commentUpdateDTO, CancellationToken cancellationToken = default)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentUpdateDTO.Id);
        if (comment == null)
        {
            return false;
        }
        comment.Content = commentUpdateDTO.Content;
        comment.Title = commentUpdateDTO.Title;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteComment(Guid commentId, CancellationToken cancellationToken = default)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
        if (comment == null)
        {
            return false;
        }
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteCommentByPostId(Guid postId, CancellationToken cancellationToken = default)
    {
        var comments = await _context.Comments.Where(c => c.PostId == postId).ToListAsync();
        if (comments == null)
        {
            return false;
        }
        _context.Comments.RemoveRange(comments);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public IQueryable<CommentDTO> GetAllCommentByPostId(Guid postId)
    {
        return GetAllComments().Where(c => c.PostId == postId);
    }
}
