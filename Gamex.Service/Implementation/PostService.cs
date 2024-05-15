namespace Gamex.Service.Implementation;

public class PostService(GamexDbContext context) : IPostService
{
    private readonly GamexDbContext _context = context;

    /// <summary>
    /// Retrieves a single post by its ID.
    /// </summary>
    /// <param name="postId">The ID of the post to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The post DTO if found, otherwise null.</returns>
    public async Task<PostDTO?> GetPost(Guid postId, CancellationToken cancellationToken = default)
    {
        // TODO: Remove comment for single post
        var post = await _context.Posts.Include(x => x.User).Include(x => x.Picture).Include(x => x.PostTags).ThenInclude(x => x.Tag).Include(x => x.Comments).ThenInclude(x => x.User).AsNoTracking().FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
        return post is not null ? MapPostToDTO(post) : null;

    }

    /// <summary>
    /// Retrieves all posts.
    /// </summary>
    /// <returns>The queryable collection of post DTOs.</returns>
    public IQueryable<PostDTO> GetAllPosts()
    {
        var posts = _context.Posts.Include(x => x.User).Include(x => x.Picture).Include(x => x.PostTags).ThenInclude(x => x.Tag).Include(x => x.Comments).ThenInclude(x => x.User).AsNoTracking();
        return posts.Select(p => MapPostToDTO(p));
    }

    /// <summary>
    /// Retrieves all posts based on the specified criteria.
    /// </summary>
    /// <param name="TagIds">The collection of tag IDs to filter by.</param>
    /// <param name="take">The number of posts to take.</param>
    /// <param name="skip">The number of posts to skip.</param>
    /// <param name="s">The search string to filter by.</param>
    /// <returns>The queryable collection of post DTOs.</returns>
    public async Task<PaginationDTO<PostDTO>> GetAllPosts(IEnumerable<string> TagIds, int take = 10, int skip = 0, string s = "", CancellationToken cancellationToken = default)
    {
        var posts = _context.Posts.Include(x => x.User).Include(x => x.Picture).Include(x => x.PostTags).ThenInclude(x => x.Tag).Include(x => x.Comments).ThenInclude(x => x.User).AsNoTracking();
        if (TagIds.Any())
        {
            TagIds = TagIds.Select(t => t.ToLower().ToString());
            posts = posts.Where(p => p.PostTags.Any(t => TagIds.Contains(t.Tag.Name.ToLower().ToString())));
        }
        if (!string.IsNullOrWhiteSpace(s))
        {
            posts = posts.Where(p => p.Title.Contains(s, StringComparison.CurrentCultureIgnoreCase) || p.Content.Contains(s, StringComparison.CurrentCultureIgnoreCase));
        }
        var allPosts = posts.OrderByDescending(p => p.DateCreated).Skip(skip).Take(take).Select(p => MapPostToDTO(p));
        var totalCount = await posts.CountAsync(cancellationToken);
        PaginationDTO<PostDTO> paginationDTO = new(await allPosts.ToListAsync(cancellationToken), Math.Ceiling((decimal)totalCount / take), skip, take, totalCount);
        return paginationDTO;
    }

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="postCreateDTO">The post creation DTO.</param>
    /// <param name="user">The user creating the post.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the post is created successfully, otherwise false.</returns>
    public async Task<bool> CreatePost(PostCreateDTO postCreateDTO, ApplicationUser user, CancellationToken cancellationToken = default)
    {
        var executionStrategy = _context.Database.CreateExecutionStrategy();
        await executionStrategy.Execute(
            async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
                bool hasSaved = false;
                try
                {
                    var post = new Post()
                    {
                        Title = postCreateDTO.Title,
                        Content = postCreateDTO.Content,
                        DateCreated = DateTime.UtcNow,
                        DateModified = DateTime.UtcNow,
                        UserId = user.Id,
                        PictureId = postCreateDTO.PictureId

                    };
                    await _context.Posts.AddAsync(post);
                    await _context.SaveChangesAsync(cancellationToken);
                    hasSaved = true;

                    if (postCreateDTO.TagIds is not null)
                    {
                        var postTag = await _context.Tags.Where(t => postCreateDTO.TagIds.Contains(t.Id))
                            .Select(t => new PostTag()
                            {
                                PostId = post.Id,
                                TagId = t.Id
                            }).ToListAsync();
                        await _context.PostTags.AddRangeAsync(postTag);
                        await _context.SaveChangesAsync(cancellationToken);
                        hasSaved = true;
                    }
                    await transaction.CommitAsync(cancellationToken);
                    return true;
                }
                catch (Exception)
                {
                    if (hasSaved)
                        await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        return false;
    }

    /// <summary>
    /// Creates a new post (mock version).
    /// </summary>
    /// <param name="postCreateDTO">The post creation DTO.</param>
    /// <param name="user">The user creating the post.</param>
    /// <returns>True if the post is created successfully, otherwise false.</returns>
    public async Task<bool> CreatePostMock(PostCreateDTO postCreateDTO, ApplicationUser user)
    {
        var post = new Post()
        {
            Title = postCreateDTO.Title,
            Content = postCreateDTO.Content,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
            UserId = user.Id,
            PictureId = postCreateDTO.PictureId

        };
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();

        if (postCreateDTO.TagIds is not null)
        {
            var postTag = await _context.Tags.Where(t => postCreateDTO.TagIds.Contains(t.Id))
                .Select(t => new PostTag()
                {
                    PostId = post.Id,
                    TagId = t.Id
                }).ToListAsync();
            await _context.PostTags.AddRangeAsync(postTag);
            await _context.SaveChangesAsync();
        }
        return true;

    }

    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="postUpdateDTO">The post update DTO.</param>
    /// <param name="user">The user updating the post.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the post is updated successfully, otherwise false.</returns>
    public async Task<bool> UpdatePost(PostUpdateDTO postUpdateDTO, ApplicationUser user, CancellationToken cancellationToken = default)
    {
        var executionStrategy = _context.Database.CreateExecutionStrategy();
        await executionStrategy.Execute(
            async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
                bool hasSaved = false;
                try
                {
                    var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postUpdateDTO.Id && p.UserId == user.Id, cancellationToken);
                    if (post is not null)
                    {
                        post.Title = postUpdateDTO.Title;
                        post.Content = postUpdateDTO.Content;
                        post.DateModified = DateTime.UtcNow;
                        await _context.SaveChangesAsync(cancellationToken);
                        hasSaved = true;
                        if (postUpdateDTO.TagIds is not null)
                        {
                            var postTags = await _context.PostTags.Where(pt => pt.PostId == post.Id).ToListAsync();
                            _context.PostTags.RemoveRange(postTags);
                            await _context.SaveChangesAsync(cancellationToken);

                            var newPostTags = await _context.Tags.Where(t => postUpdateDTO.TagIds.Contains(t.Id))
                                .Select(t => new PostTag()
                                {
                                    PostId = post.Id,
                                    TagId = t.Id
                                }).ToListAsync();
                            await _context.PostTags.AddRangeAsync(newPostTags, cancellationToken);
                            await _context.SaveChangesAsync(cancellationToken);
                            hasSaved = true;
                        }
                        await transaction.CommitAsync(cancellationToken);
                        return true;
                    }
                }
                catch (Exception)
                {
                    if (hasSaved)
                        await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
                return false;
            });
        return false;
    }

    /// <summary>
    /// Updates an existing post (mock version).
    /// </summary>
    /// <param name="postUpdateDTO">The post update DTO.</param>
    /// <param name="user">The user updating the post.</param>
    /// <returns>True if the post is updated successfully, otherwise false.</returns>
    public async Task<bool> UpdatePostMock(PostUpdateDTO postUpdateDTO, ApplicationUser user)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postUpdateDTO.Id && p.UserId == user.Id);
        if (post is not null)
        {
            post.Title = postUpdateDTO.Title;
            post.Content = postUpdateDTO.Content;
            post.DateModified = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            if (postUpdateDTO.TagIds is not null)
            {
                var postTags = await _context.PostTags.Where(pt => pt.PostId == post.Id).ToListAsync();
                _context.PostTags.RemoveRange(postTags);
                await _context.SaveChangesAsync();

                var newPostTags = await _context.Tags.Where(t => postUpdateDTO.TagIds.Contains(t.Id))
                    .Select(t => new PostTag()
                    {
                        PostId = post.Id,
                        TagId = t.Id
                    }).ToListAsync();
                await _context.PostTags.AddRangeAsync(newPostTags);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Deletes a post by its ID.
    /// </summary>
    /// <param name="postId">The ID of the post to delete.</param>
    /// <param name="user">The user deleting the post.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the post is deleted successfully, otherwise false.</returns>
    public async Task<bool> DeletePost(Guid postId, ApplicationUser user, CancellationToken cancellationToken = default)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId && p.UserId == user.Id, cancellationToken);
        if (post == null)
        {
            return false;
        }
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Archives a post by its ID.
    /// </summary>
    /// <param name="postId">The ID of the post to archive.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the post is archived successfully, otherwise false.</returns>
    public async Task<bool> ArchivePost(Guid postId, CancellationToken cancellationToken = default)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
        if (post is not null)
        {
            post.IsArchived = true;
            await _context.SaveChangesAsync(cancellationToken);
        }
        return true;
    }

    /// <summary>
    /// Unarchives a post by its ID.
    /// </summary>
    /// <param name="postId">The ID of the post to unarchive.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the post is unarchived successfully, otherwise false.</returns>
    public async Task<bool> UnArchivePost(Guid postId, CancellationToken cancellationToken = default)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
        if (post is not null)
        {
            post.IsArchived = false;
            await _context.SaveChangesAsync(cancellationToken);
        }
        return true;
    }

    /// <summary>
    /// Retrieves all posts for a specific user based on the specified criteria.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="TagIds">The collection of tag IDs to filter by.</param>
    /// <param name="take">The number of posts to take.</param>
    /// <param name="skip">The number of posts to skip.</param>
    /// <param name="s">The search string to filter by.</param>
    /// <returns>The pagination DTO containing the queryable collection of post DTOs.</returns>
    public async Task<PaginationDTO<PostDTO>> GetAllUsersPosts(string userId, IEnumerable<string> TagIds, int take = 10, int skip = 0, string s = "", CancellationToken cancellationToken = default)
    {
        var posts = _context.Posts.Include(x => x.User).Include(x => x.Picture).Include(x => x.PostTags).ThenInclude(x => x.Tag).Include(x => x.Comments).ThenInclude(x => x.User).Where(x => x.UserId == userId).AsNoTracking();
        if (TagIds.Any())
        {
            TagIds = TagIds.Select(t => t.ToLower().ToString());
            posts = posts.Where(p => p.PostTags.Any(t => TagIds.Contains(t.Tag.Name.ToLower().ToString())));
        }
        if (!string.IsNullOrWhiteSpace(s))
        {
            posts = posts.Where(p => p.Title.Contains(s, StringComparison.CurrentCultureIgnoreCase) || p.Content.Contains(s, StringComparison.CurrentCultureIgnoreCase));
        }
        var allPosts = posts.OrderByDescending(p => p.DateCreated).Skip(skip).Take(take).Select(p => MapPostToDTO(p));
        var totalCount = await posts.CountAsync(cancellationToken);
        PaginationDTO<PostDTO> paginationDTO = new(await allPosts.ToListAsync(cancellationToken), Math.Ceiling((decimal)totalCount / take), skip, take, totalCount);
        return paginationDTO;
    }

    /// <summary>
    /// Maps a <see cref="Post"/> entity to a <see cref="PostDTO"/> data transfer object.
    /// </summary>
    /// <param name="post">The <see cref="Post"/> entity to map.</param>
    /// <returns>The mapped <see cref="PostDTO"/>.</returns>
    private static PostDTO MapPostToDTO(Post post)
    {
        var comments = post.Comments?.Select(c => new CommentDTO(c.Id, c.Title, c.Content, c.IsArchived, new UserProfileDTO(c.User?.FirstName, c.User?.LastName, c.User?.DisplayName, c.User?.Email, c?.User?.PhoneNumber, c?.User?.Picture?.FileUrl, c?.User?.Picture?.PublicId, c?.User?.EmailConfirmed ?? false), c.PostId, c.DateCreated));

        var tags = post.PostTags?.Select(pt => new TagDTO(pt.Tag.Id, pt.Tag.Name, pt.Tag.PostTags.Count));

        return new PostDTO(post.Id, post.Title, post.Content, post.IsArchived, post?.Picture?.Id, post?.Picture?.FileUrl, post?.Picture?.PublicId,
            new UserProfileDTO(post?.User?.FirstName, post?.User?.LastName, post?.User?.DisplayName, post?.User?.Email, post?.User?.PhoneNumber, post?.User?.Picture?.FileUrl, post?.User?.Picture?.PublicId, post?.User?.EmailConfirmed ?? false), comments, tags, post.DateCreated);
    }
}
