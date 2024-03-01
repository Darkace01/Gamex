namespace Gamex.Controllers;
[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/leaderboard")]
[ApiController]
public class LeaderboardController(IRepositoryServiceManager repo, UserManager<ApplicationUser> userManager) : BaseController(userManager, repo)
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LeaderboardDTO>>), StatusCodes.Status200OK)]
    public IActionResult GetLeaderboard()
    {
        var leaderboard = _repositoryServiceManager.LeaderboardService.GetLeaderboard();
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<LeaderboardDTO>>(leaderboard));
    }
}
