using System.Threading;

namespace Gamex.Service.Implementation;

public class PostService(GamexDbContext context) : IPostService
{
    private readonly GamexDbContext _context = context;

    public async Task<PostDTO?> GetPost(Guid postId, CancellationToken cancellationToken = default)
    {
        return await GetAllPosts().FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
    }

    public IQueryable<PostDTO> GetAllPosts()
    {
        var posts = _context.Posts.Include(x => x.User).Include(x => x.Picture).Include(x => x.PostTags).ThenInclude(x => x.Tag).Include(x => x.Comments).ThenInclude(x => x.User).AsNoTracking();
        return posts.Select(p => MapPostToDTO(p));
    }
    public IQueryable<PostDTO> GetAllPosts(IEnumerable<string> TagIds, int take = 10, int skip = 0, string s = "")
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
        return posts.OrderByDescending(p => p.DateCreated).Skip(skip).Take(take).Select(p => MapPostToDTO(p));
    }

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

    private static PostDTO MapPostToDTO(Post post)
    {
        var comments = post.Comments?.Select(c => new CommentDTO(c.Id, c.Title, c.Content, c.IsArchived, new UserProfileDTO(c.User?.FirstName, c.User?.LastName, c.User?.DisplayName, c.User?.Email, c?.User?.PhoneNumber, c?.User?.Picture?.FileUrl, c?.User?.Picture?.PublicId), c.PostId, c.DateCreated));

        var tags = post.PostTags?.Select(pt => new TagDTO(pt.Tag.Id, pt.Tag.Name, pt.Tag.PostTags.Count));

        return new PostDTO(post.Id, post.Title, post.Content, post.IsArchived, post?.Picture?.Id, post?.Picture?.FileUrl, post?.Picture?.PublicId,
            new UserProfileDTO(post?.User?.FirstName, post?.User?.LastName, post?.User?.DisplayName, post?.User?.Email, post?.User?.PhoneNumber, post?.User?.Picture?.FileUrl, post?.User?.Picture?.PublicId), comments, tags, post.DateCreated);
    }
}
