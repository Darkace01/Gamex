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
    public async Task<IActionResult> GetLeaderboard([FromQuery] string tournamentId = "")
    {
        if (string.IsNullOrWhiteSpace(tournamentId))
        {
            var cachedLeaderboard = await _repositoryServiceManager.CacheService.GetOrCreateAsync(
                $"{nameof(GetLeaderboard)}{tournamentId}",
                () => Task.FromResult(_repositoryServiceManager.LeaderboardService.GetLeaderboard())
            );
            return Ok(new ApiResponse<IEnumerable<LeaderboardDTO>>(cachedLeaderboard));
        }

        if (Guid.TryParse(tournamentId, out Guid tournamentGuid))
        {
            var cachedLeaderboardWithTournamentFilter = await _repositoryServiceManager.CacheService.GetOrCreateAsync(
               $"{nameof(GetLeaderboard)}{tournamentId}",
               () => Task.FromResult(_repositoryServiceManager.LeaderboardService.GetLeaderboardWithTournamentFilter(tournamentGuid)),
               TimeSpan.FromMinutes(20)
           );
            return Ok(new ApiResponse<IEnumerable<LeaderboardDTO>>(cachedLeaderboardWithTournamentFilter));
        }

        return BadRequest(new ApiResponse<IEnumerable<LeaderboardDTO>>(StatusCodes.Status400BadRequest, "Invalid tournament ID"));
    }
}
