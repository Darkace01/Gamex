using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gamex.Controllers;
[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/leaderboard")]
[ApiController]
public class LeaderboardController(IRepositoryServiceManager repo) : ControllerBase
{
    private readonly IRepositoryServiceManager _repo = repo;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LeaderboardDTO>>), StatusCodes.Status200OK)]
    public IActionResult GetLeaderboard([FromQuery]string tournamentId = "")
    {
        if (string.IsNullOrWhiteSpace(tournamentId))
        {
            var leaderboard = _repositoryServiceManager.LeaderboardService.GetLeaderboard();
            return Ok(new ApiResponse<IEnumerable<LeaderboardDTO>>(leaderboard));
        }

        if (Guid.TryParse(tournamentId, out Guid tournamentGuid))
        {
            var leaderboardWithTournamentFilter = _repositoryServiceManager.LeaderboardService.GetLeaderboardWithTournamentFilter(tournamentGuid);
            return Ok(new ApiResponse<IEnumerable<LeaderboardDTO>>(leaderboardWithTournamentFilter));
        }

        return BadRequest("Invalid tournamentId");
    }
}
