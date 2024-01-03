using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gamex.Controllers;
[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/[controller]")]
[ApiController]
public class TournamentController(IRepositoryServiceManager repositoryServiceManager) : ControllerBase
{
    private readonly IRepositoryServiceManager _repositoryServiceManager = repositoryServiceManager;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TournamentDTO>>), StatusCodes.Status200OK)]
    public IActionResult GetTournaments()
    {
        var tournaments = _repositoryServiceManager.TournamentService.GetAllTournaments();
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<TournamentDTO>>(tournaments));
    }
}
