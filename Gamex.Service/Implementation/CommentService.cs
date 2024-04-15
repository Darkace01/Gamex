﻿using Gamex.Models;

namespace Gamex.Service.Implementation;

/// <summary>
/// Initializes a new instance of the <see cref="CommentService"/> class.
/// </summary>
/// <param name="context">The database context.</param>
public class CommentService(GamexDbContext context) : ICommentService
{
    private readonly GamexDbContext _context = context;

    /// <summary>
    /// Gets a comment by its ID.
    /// </summary>
    /// <param name="id">The ID of the comment.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The comment with the specified ID, or null if not found.</returns>
    public async Task<CommentDTO?> GetCommentById(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAllComments().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    /// <summary>
    /// Gets all comments.
    /// </summary>
    /// <returns>The queryable collection of comments.</returns>
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

    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="commentCreateDTO">The DTO containing the comment data.</param>
    /// <param name="userId">The ID of the user creating the comment.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the comment is created successfully, otherwise false.</returns>
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

    /// <summary>
    /// Updates an existing comment.
    /// </summary>
    /// <param name="commentUpdateDTO">The DTO containing the updated comment data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the comment is updated successfully, otherwise false.</returns>
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

    /// <summary>
    /// Deletes a comment by its ID.
    /// </summary>
    /// <param name="commentId">The ID of the comment to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the comment is deleted successfully, otherwise false.</returns>
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

    /// <summary>
    /// Deletes all comments associated with a post.
    /// </summary>
    /// <param name="postId">The ID of the post.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the comments are deleted successfully, otherwise false.</returns>
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

    /// <summary>
    /// Gets all comments associated with a post.
    /// </summary>
    /// <param name="postId">The ID of the post.</param>
    /// <returns>The queryable collection of comments associated with the post.</returns>
    public IQueryable<CommentDTO> GetAllCommentByPostId(Guid postId)
    {
        return GetAllComments().Where(c => c.PostId == postId);
    }
}
