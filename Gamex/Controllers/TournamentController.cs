namespace Gamex.Controllers;
[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/tournaments")]
[ApiController]
public class TournamentController(IRepositoryServiceManager repositoryServiceManager, UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly IRepositoryServiceManager _repositoryServiceManager = repositoryServiceManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<PaginatedTournamentDTO>), StatusCodes.Status200OK)]
    public IActionResult GetTournaments([FromQuery] int take = 10, [FromQuery] int skip = 0)
    {
        var tournaments = _repositoryServiceManager.TournamentService.GetAllTournaments();
        var totalNumber = tournaments.Count();
        tournaments = tournaments.Skip(skip).Take(take);
        var paginationMetadata = new PaginatedTournamentDTO
        {
            TotalCount = totalNumber,
            PageSize = take,
            CurrentPage = skip,
            TotalPages = Math.Ceiling((decimal)totalNumber / take),
            Tournaments = tournaments
        };
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<PaginatedTournamentDTO>(paginationMetadata));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<TournamentDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTournament(Guid id)
    {
        var tournament = await _repositoryServiceManager.TournamentService.GetTournamentById(id);
        if (tournament == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<TournamentDTO>(404, "Tournament not found"));

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<TournamentDTO>(tournament));
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTournament([FromBody] TournamentCreateDTO tournamentCreateDTO)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        await _repositoryServiceManager.TournamentService.CreateTournament(tournamentCreateDTO, user);
        
        return StatusCode(StatusCodes.Status201Created, new ApiResponse<string>("Tournament Successfully Created"));
    }

    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateTournament(Guid id, [FromBody] TournamentUpdateDTO tournamentUpdateDTO)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        var tournamentExist = await _repositoryServiceManager.TournamentService.GetTournamentById(id);
        if (tournamentExist == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>(404, "Tournament not found"));

        if(tournamentExist.TournamentUsers.All(ut => ut.UserId != user.Id))
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        await _repositoryServiceManager.TournamentService.UpdateTournament(tournamentUpdateDTO, user);

        return StatusCode(StatusCodes.Status204NoContent, new ApiResponse<string>("Tournament Successfully Updated"));
    }

    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTournament(Guid id)
    {
        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        var tournamentExist = await _repositoryServiceManager.TournamentService.GetTournamentById(id);
        if (tournamentExist == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>(404, "Tournament not found"));

        if (tournamentExist.TournamentUsers.All(ut => ut.UserId != user.Id))
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        await _repositoryServiceManager.TournamentService.DeleteTournament(id, user);

        return StatusCode(StatusCodes.Status204NoContent, new ApiResponse<string>("Tournament Successfully Deleted"));
    }

    [HttpPost("{id}/join")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
    public async Task<IActionResult> JoinTournament(Guid id)
    {
        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        var tournamentExist = await _repositoryServiceManager.TournamentService.GetTournamentById(id);
        if (tournamentExist == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>(404, "Tournament not found"));

        if (tournamentExist.TournamentUsers.Any(ut => ut.UserId == user.Id))
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Your are already in this tournament"));

        await _repositoryServiceManager.TournamentService.JoinTournament(id, user);

        return StatusCode(StatusCodes.Status201Created, new ApiResponse<string>("Tournament Successfully Joined"));
    }

    [HttpGet("featured")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TournamentDTO>>), StatusCodes.Status200OK)]
    public IActionResult GetFeaturedTournaments()
    {
        var tournaments = _repositoryServiceManager.TournamentService.GetFeaturedTournaments();
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<TournamentDTO>>(tournaments));
    }
    #region Helpers
    private async Task<ApplicationUser?> GetUser()
    {
        var username = User?.Identity?.Name;
        var user = await _userManager.FindByNameAsync(username);
        return user;
    }
    #endregion
}
