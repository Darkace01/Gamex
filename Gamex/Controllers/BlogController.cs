using Gamex.DTO;
using Microsoft.AspNetCore.Identity;

namespace Gamex.Controllers;
[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/blog")]
[ApiController]
public class BlogController(IRepositoryServiceManager repo, UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly IRepositoryServiceManager _repo = repo;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpGet("posts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PostDTO>>), StatusCodes.Status200OK)]
    public IActionResult GetPosts()
    {
        var posts = _repo.PostService.GetAllPosts();
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<PostDTO>>(posts));
    }

    [HttpGet("posts/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<PostDTO>), StatusCodes.Status200OK)]
    public IActionResult GetPost(Guid id)
    {
        var post = _repo.PostService.GetPost(id);
        if (post == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<PostDTO>(404, "Post not found"));

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<PostDTO>(post));
    }

    [HttpPost("posts")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePost([FromForm] PostCreateDTO postCreateDTO)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        if(postCreateDTO.Picture is not null)
        {
            var uploadResult = await _repo.FileStorageService.SaveFile(postCreateDTO.Picture, AppConstant.PostPictureTag);
            if (uploadResult is not null)
            {
                var pictureFile = await _repo.PictureService.CreatePicture(new PictureCreateDTO(uploadResult.FileUrl, uploadResult.PublicId));
                postCreateDTO.PictureId = pictureFile.Id;
            }
        }

        await _repo.PostService.CreatePost(postCreateDTO, user);

        return StatusCode(StatusCodes.Status201Created, new ApiResponse<string>("Post Successfully Created"));
    }

    [HttpPut("posts/{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePost(Guid id, [FromBody] PostUpdateDTO postUpdateDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
            return Unauthorized(new ApiResponse<string>(401, "Unauthorized"));

        var postToUpdate = _repo.PostService.GetPost(id);
        if (postToUpdate == null)
            return NotFound(new ApiResponse<string>(404, "Post not found"));

        if (postToUpdate.User.Email != user.Email)
            return Unauthorized(new ApiResponse<string>(401, "Unauthorized"));

        if (postUpdateDTO.Picture is not null)
        {
            var uploadResult = await _repo.FileStorageService.SaveFile(postUpdateDTO.Picture, AppConstant.PostPictureTag);
            if (uploadResult is not null)
            {
                if (!string.IsNullOrWhiteSpace(postToUpdate.PicturePublicId))
                {
                    await _repo.FileStorageService.DeleteFile(postToUpdate.PicturePublicId);
                }

                if (!string.IsNullOrWhiteSpace(uploadResult.PublicId))
                {
                    var pictureFileToUpdate = await _repo.PictureService.GetPictureByPublicId(uploadResult.PublicId);
                    await _repo.PictureService.UpdatePicture(new PictureUpdateDTO(pictureFileToUpdate.Id, uploadResult.FileUrl, uploadResult.PublicId));
                    postUpdateDTO.PictureId = pictureFileToUpdate.Id;
                }
                else
                {
                    var pictureFile = await _repo.PictureService.CreatePicture(new PictureCreateDTO(uploadResult.FileUrl, uploadResult.PublicId));
                    postUpdateDTO.PictureId = pictureFile.Id;
                }
            }
        }

        await _repo.PostService.UpdatePost(postUpdateDTO, user);

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>("Post Successfully Updated"));
    }

    [HttpDelete("posts/{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        var postToDelete = _repo.PostService.GetPost(id);
        if (postToDelete == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>(404, "Post not found"));

        if (postToDelete.User.Email != user.Email)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        await _repo.PostService.DeletePost(id, user);

        return StatusCode(StatusCodes.Status204NoContent, new ApiResponse<string>("Post Successfully Deleted"));
    }

    [HttpGet("posts/{id}/comments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CommentDTO>>), StatusCodes.Status200OK)]
    public IActionResult GetPostComments(Guid id)
    {
        var comments = _repo.CommentService.GetAllCommentByPostId(id);
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<CommentDTO>>(comments));
    }

    [HttpGet("comments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CommentDTO>>), StatusCodes.Status200OK)]
    public IActionResult GetComments()
    {
        var comments = _repo.CommentService.GetAllComments();
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<CommentDTO>>(comments));
    }

    [HttpGet("comments/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<CommentDTO>), StatusCodes.Status200OK)]
    public IActionResult GetComment(Guid id)
    {
        var comment = _repo.CommentService.GetCommentById(id);
        if (comment == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<CommentDTO>(404, "Comment not found"));

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<CommentDTO>(comment));
    }

    [HttpPost("comments")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateComment([FromBody] CommentCreateDTO commentCreateDTO)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        await _repo.CommentService.CreateComment(commentCreateDTO, user.Id);

        return StatusCode(StatusCodes.Status201Created, new ApiResponse<string>("Comment Successfully Created"));
    }

    [HttpPut("comments/{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateComment(Guid id, [FromBody] CommentUpdateDTO commentUpdateDTO)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        var commentToUpdate = _repo.CommentService.GetCommentById(id);
        if (commentToUpdate == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>(404, "Comment not found"));

        if (commentToUpdate.User.Email != user.Email)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        await _repo.CommentService.UpdateComment(commentUpdateDTO);

        return StatusCode(StatusCodes.Status204NoContent, new ApiResponse<string>("Comment Successfully Updated"));
    }

    [HttpDelete("comments/{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        var commentToDelete = _repo.CommentService.GetCommentById(id);
        if (commentToDelete == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>(404, "Comment not found"));

        if (commentToDelete.User.Email != user.Email)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        await _repo.CommentService.DeleteComment(id);

        return StatusCode(StatusCodes.Status204NoContent, new ApiResponse<string>("Comment Successfully Deleted"));
    }

    [HttpGet("tags")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TagDTO>>), StatusCodes.Status200OK)]
    public IActionResult GetTags()
    {
        var tags = _repo.TagService.GetAllTags();
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<TagDTO>>(tags));
    }

    [HttpGet("tags/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<TagDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTag(Guid id)
    {
        var tag = await _repo.TagService.GetTagById(id);
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

        var tagExist = await _repo.TagService.GetTagByName(tagCreateDTO.Name);
        if (tagExist != null)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Tag already exist"));

        var tag = await _repo.TagService.CreateTag(tagCreateDTO);

        return StatusCode(StatusCodes.Status201Created, new ApiResponse<string>("Tag Successfully Created"));
    }

    #region Helpers
    private async Task<ApplicationUser?> GetUser()
    {
        var username = User?.Identity?.Name;
        if (username is null)
            return null;
        var user = await _userManager.FindByNameAsync(username);
        return user;
    }
    #endregion
}
