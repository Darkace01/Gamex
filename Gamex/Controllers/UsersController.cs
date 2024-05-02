namespace Gamex.Controllers;
[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/users")]
[ApiController]
public class UsersController(IRepositoryServiceManager repositoryServiceManager, UserManager<ApplicationUser> userManager) : BaseController(userManager, repositoryServiceManager)
{
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<UserPublicProfileDTO>), StatusCodes.Status200OK)]
    public IActionResult GetUserById(string id)
    {
        var user = repositoryServiceManager.ExtendedUserService.GetPublicUserByIdForProfile(id);
        if (user is null) return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "User not found"));
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<UserPublicProfileDTO>(user, 200, "User retrieved"));
    }
}
