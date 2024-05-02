namespace Gamex.Controllers;
[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/blog")]
[ApiController]
public class BlogController(IRepositoryServiceManager repo, UserManager<ApplicationUser> userManager) : BaseController(userManager, repo)
{
    [HttpGet("posts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<PaginationDTO<PostDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPosts([FromQuery] IEnumerable<string> TagIds, [FromQuery] int take = 10, [FromQuery] int skip = 0, [FromQuery] string s = "", CancellationToken cancellationToken = default)
    {
        var postList = await _repositoryServiceManager.PostService.GetAllPosts(TagIds, take, skip, s, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<PaginationDTO<PostDTO>>(postList));
    }

    [HttpGet("posts/user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<PaginationDTO<PostDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPosts([FromRoute] string userId, [FromQuery] IEnumerable<string> TagIds, [FromQuery] int take = 10, [FromQuery] int skip = 0, [FromQuery] string s = "", CancellationToken cancellationToken = default)
    {
        var postList = await _repositoryServiceManager.PostService.GetAllUsersPosts(userId, TagIds, take, skip, s, cancellationToken);
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<PaginationDTO<PostDTO>>(postList));
    }

    [HttpGet("posts/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<PostDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPost(Guid id, CancellationToken cancellationToken = default)
    {
        var post = await _repositoryServiceManager.PostService.GetPost(id, cancellationToken);
        if (post == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<PostDTO>(404, "Post not found"));

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<PostDTO>(post));
    }

    [HttpPost("posts")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePost([FromForm] PostCreateDTO postCreateDTO, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        if (postCreateDTO.Picture is not null)
        {
            var uploadResult = await _repositoryServiceManager.FileStorageService.SaveFile(postCreateDTO.Picture, AppConstant.PostPictureTag);
            if (uploadResult is not null)
            {
                var pictureFile = await _repositoryServiceManager.PictureService.CreatePicture(new PictureCreateDTO(uploadResult.FileUrl, uploadResult.PublicId));
                postCreateDTO.PictureId = pictureFile.Id;
            }
        }

        await _repositoryServiceManager.PostService.CreatePost(postCreateDTO, user, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, new ApiResponse<string>("Post Successfully Created"));
    }

    [HttpPut("posts/{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePost(Guid id, [FromBody] PostUpdateDTO postUpdateDTO, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
            return Unauthorized(new ApiResponse<string>(401, "Unauthorized"));

        var postToUpdate = await _repositoryServiceManager.PostService.GetPost(id, cancellationToken);
        if (postToUpdate == null)
            return NotFound(new ApiResponse<string>(404, "Post not found"));

        if (postToUpdate.User.Email != user.Email)
            return Unauthorized(new ApiResponse<string>(401, "Unauthorized"));

        if (postUpdateDTO.Picture is not null)
        {
            var uploadResult = await _repositoryServiceManager.FileStorageService.SaveFile(postUpdateDTO.Picture, AppConstant.PostPictureTag);
            if (uploadResult is not null)
            {
                if (!string.IsNullOrWhiteSpace(postToUpdate.PicturePublicId))
                {
                    await _repositoryServiceManager.FileStorageService.DeleteFile(postToUpdate.PicturePublicId);
                }

                if (!string.IsNullOrWhiteSpace(uploadResult.PublicId))
                {
                    var pictureFileToUpdate = await _repositoryServiceManager.PictureService.GetPictureByPublicId(uploadResult.PublicId);
                    await _repositoryServiceManager.PictureService.UpdatePicture(new PictureUpdateDTO(pictureFileToUpdate.Id, uploadResult.FileUrl, uploadResult.PublicId));
                    postUpdateDTO.PictureId = pictureFileToUpdate.Id;
                }
                else
                {
                    var pictureFile = await _repositoryServiceManager.PictureService.CreatePicture(new PictureCreateDTO(uploadResult.FileUrl, uploadResult.PublicId));
                    postUpdateDTO.PictureId = pictureFile.Id;
                }
            }
        }

        await _repositoryServiceManager.PostService.UpdatePost(postUpdateDTO, user, cancellationToken);

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>("Post Successfully Updated"));
    }

    [HttpDelete("posts/{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeletePost(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        var postToDelete = await _repositoryServiceManager.PostService.GetPost(id, cancellationToken);
        if (postToDelete == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>(404, "Post not found"));

        if (postToDelete.User.Email != user.Email)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        await _repositoryServiceManager.PostService.DeletePost(id, user, cancellationToken);

        return StatusCode(StatusCodes.Status204NoContent, new ApiResponse<string>("Post Successfully Deleted"));
    }

    [HttpGet("posts/{id}/comments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<PaginationDTO<CommentDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPostComments(Guid id, [FromQuery] int take = 10, [FromQuery] int skip = 0, CancellationToken cancellationToken = default)
    {
        var comments = _repositoryServiceManager.CommentService.GetAllCommentByPostId(id);
        var totalNumber = await comments.CountAsync(cancellationToken);

        var commentList = await comments.Skip(skip).Take(take).OrderByDescending(x => x.DateCreated).ToListAsync(cancellationToken);
        PaginationDTO<CommentDTO> pagination = new(commentList, Math.Ceiling((decimal)totalNumber / take), skip, take, totalNumber);
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<PaginationDTO<CommentDTO>>(pagination));
    }

    [HttpGet("comments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<PaginationDTO<CommentDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetComments([FromQuery] int take = 10, [FromQuery] int skip = 0, CancellationToken cancellationToken = default)
    {
        var comments = _repositoryServiceManager.CommentService.GetAllComments();
        var totalNumber = await comments.CountAsync(cancellationToken);

        var commentList = await comments.Skip(skip).Take(take).OrderByDescending(x => x.DateCreated).ToListAsync(cancellationToken);
        PaginationDTO<CommentDTO> pagination = new(commentList, Math.Ceiling((decimal)totalNumber / take), skip, take, totalNumber);
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<PaginationDTO<CommentDTO>>(pagination));
    }

    [HttpGet("comments/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<CommentDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetComment(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await _repositoryServiceManager.CommentService.GetCommentById(id, cancellationToken);
        if (comment == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<CommentDTO>(404, "Comment not found"));

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<CommentDTO>(comment));
    }

    [HttpPost("comments")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateComment([FromBody] CommentCreateDTO commentCreateDTO, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        await _repositoryServiceManager.CommentService.CreateComment(commentCreateDTO, user.Id, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, new ApiResponse<string>("Comment Successfully Created"));
    }

    [HttpPut("comments/{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateComment(Guid id, [FromBody] CommentUpdateDTO commentUpdateDTO, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        var commentToUpdate = await _repositoryServiceManager.CommentService.GetCommentById(id, cancellationToken);
        if (commentToUpdate == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>(404, "Comment not found"));

        if (commentToUpdate.User.Email != user.Email)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        await _repositoryServiceManager.CommentService.UpdateComment(commentUpdateDTO, cancellationToken);

        return StatusCode(StatusCodes.Status204NoContent, new ApiResponse<string>("Comment Successfully Updated"));
    }

    [HttpDelete("comments/{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteComment(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        var commentToDelete = await _repositoryServiceManager.CommentService.GetCommentById(id, cancellationToken);
        if (commentToDelete == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>(404, "Comment not found"));

        if (commentToDelete.User.Email != user.Email)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        await _repositoryServiceManager.CommentService.DeleteComment(id, cancellationToken);

        return StatusCode(StatusCodes.Status204NoContent, new ApiResponse<string>("Comment Successfully Deleted"));
    }

    [HttpGet("tags")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TagDTO>>), StatusCodes.Status200OK)]
    public IActionResult GetTags()
    {
        var tags = _repositoryServiceManager.TagService.GetAllTags();
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<TagDTO>>(tags));
    }

    [HttpGet("tags/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<TagDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTag(Guid id)
    {
        var tag = await _repositoryServiceManager.TagService.GetTagById(id);
        if (tag == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<TagDTO>(404, "Tag not found"));

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<TagDTO>(tag));
    }

    [HttpPost("tags")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTag([FromBody] TagCreateDTO tagCreateDTO)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid model object"));

        var tagExist = await _repositoryServiceManager.TagService.GetTagByName(tagCreateDTO.Name);
        if (tagExist != null)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Tag already exist"));

        var tag = await _repositoryServiceManager.TagService.CreateTag(tagCreateDTO);

        return StatusCode(StatusCodes.Status201Created, new ApiResponse<string>("Tag Successfully Created"));
    }
}
