namespace Gamex.Controllers;
[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/tournaments")]
[ApiController]
public class TournamentController(IRepositoryServiceManager repositoryServiceManager, UserManager<ApplicationUser> userManager) : BaseController(userManager, repositoryServiceManager)
{

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<PaginationDTO<TournamentDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTournaments([FromQuery] int take = 10, [FromQuery] int skip = 0, [FromQuery] string s = "", [FromQuery] string categoryNames = "", CancellationToken cancellationToken = default)
    {
        var tournaments = _repositoryServiceManager.TournamentService.GetAllTournaments();
        var totalNumber = tournaments.Count();

        if (!string.IsNullOrEmpty(categoryNames))
        {
            var categoryNamesList = categoryNames.Split(',').Select(x => x.ToLower()).ToList();
            if (categoryNamesList.Count != 0)
            {
                tournaments = tournaments.Where(t => t.Categories.Any() && t.Categories.Any(x => categoryNamesList.Contains(x.Name.ToLower())));
            }
        }

        if (!string.IsNullOrEmpty(s))
            tournaments = tournaments.Where(t => t.Name.Contains(s) || t.Description.Contains(s) || t.Location.Contains(s) || t.Rules.Contains(s) ||
                                                            (t.Categories.Any() && t.Categories.Any(x => x.Name.Contains(s))));

        var tournamentList = await tournaments.Skip(skip).Take(take).OrderBy(x => x.Name).ToListAsync(cancellationToken);
        PaginationDTO<TournamentDTO> paginatedTournament = new(tournamentList, Math.Ceiling((decimal)totalNumber / take), skip, take, totalNumber);

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<PaginationDTO<TournamentDTO>>(paginatedTournament));
    }

    [HttpGet("mini")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TournamentMiniDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMinimumTournament()
    {
        var miniTournament = await _repositoryServiceManager.TournamentService.GetAllTournaments()
            .Select(x => new TournamentMiniDTO(x.Id, x.Name, x.Description))
            .ToListAsync();

        return Ok(new ApiResponse<IEnumerable<TournamentMiniDTO>>(miniTournament));
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
    public async Task<IActionResult> CreateTournament([FromForm] TournamentCreateDTO tournamentCreateDTO)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        if (tournamentCreateDTO.Picture is not null)
        {
            var uploadResult = await _repositoryServiceManager.FileStorageService.SaveFile(tournamentCreateDTO.Picture, AppConstant.TournamentPictureTag);
            if (uploadResult is not null)
            {
                var pictureFile = await _repositoryServiceManager.PictureService.CreatePicture(new PictureCreateDTO(uploadResult.FileUrl, uploadResult.PublicId));
                tournamentCreateDTO.PictureId = pictureFile.Id;
            }
        }
        if (tournamentCreateDTO.CoverPicture is not null)
        {
            var uploadResult = await _repositoryServiceManager.FileStorageService.SaveFile(tournamentCreateDTO.CoverPicture, AppConstant.TournamentCoverPictureTag);
            if (uploadResult is not null)
            {
                var pictureFile = await _repositoryServiceManager.PictureService.CreatePicture(new PictureCreateDTO(uploadResult.FileUrl, uploadResult.PublicId));
                tournamentCreateDTO.CoverPictureId = pictureFile.Id;
            }
        }

        await _repositoryServiceManager.TournamentService.CreateTournament(tournamentCreateDTO, user);

        return StatusCode(StatusCodes.Status201Created, new ApiResponse<string>("Tournament Successfully Created"));
    }

    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateTournament(Guid id, [FromForm] TournamentUpdateDTO tournamentUpdateDTO)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        var tournamentExist = await _repositoryServiceManager.TournamentService.GetTournamentById(id);
        if (tournamentExist == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>(404, "Tournament not found"));

        var userRole = await _userManager.GetRolesAsync(user);

        if (!userRole.Contains(AppConstant.AdminUserRole))
            if (tournamentExist.TournamentUsers.All(ut => ut.CreatorId != user.Id))
                return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        if (tournamentUpdateDTO.Picture is not null)
        {
            var uploadResult = await _repositoryServiceManager.FileStorageService.SaveFile(tournamentUpdateDTO.Picture, AppConstant.PostPictureTag);
            if (uploadResult is not null)
            {
                if (!string.IsNullOrWhiteSpace(tournamentExist.PicturePublicId))
                {
                    await _repositoryServiceManager.FileStorageService.DeleteFile(tournamentExist.PicturePublicId);
                }

                if (!string.IsNullOrWhiteSpace(uploadResult.PublicId))
                {
                    var pictureFileToUpdate = await _repositoryServiceManager.PictureService.GetPictureByPublicId(uploadResult.PublicId);
                    await _repositoryServiceManager.PictureService.UpdatePicture(new PictureUpdateDTO(pictureFileToUpdate.Id, uploadResult.FileUrl, uploadResult.PublicId));
                    tournamentUpdateDTO.PictureId = pictureFileToUpdate.Id;
                }
                else
                {
                    var pictureFile = await _repositoryServiceManager.PictureService.CreatePicture(new PictureCreateDTO(uploadResult.FileUrl, uploadResult.PublicId));
                    tournamentUpdateDTO.PictureId = pictureFile.Id;
                }
            }
        }
        if (tournamentUpdateDTO.CoverPicture is not null)
        {
            var uploadResult = await _repositoryServiceManager.FileStorageService.SaveFile(tournamentUpdateDTO.CoverPicture, AppConstant.PostPictureTag);
            if (uploadResult is not null)
            {
                if (!string.IsNullOrWhiteSpace(tournamentExist.CoverPicturePublicId))
                {
                    await _repositoryServiceManager.FileStorageService.DeleteFile(tournamentExist.CoverPicturePublicId);
                }

                if (!string.IsNullOrWhiteSpace(uploadResult.PublicId))
                {
                    var pictureFileToUpdate = await _repositoryServiceManager.PictureService.GetPictureByPublicId(uploadResult.PublicId);
                    await _repositoryServiceManager.PictureService.UpdatePicture(new PictureUpdateDTO(pictureFileToUpdate.Id, uploadResult.FileUrl, uploadResult.PublicId));
                    tournamentUpdateDTO.CoverPictureId = pictureFileToUpdate.Id;
                }
                else
                {
                    var pictureFile = await _repositoryServiceManager.PictureService.CreatePicture(new PictureCreateDTO(uploadResult.FileUrl, uploadResult.PublicId));
                    tournamentUpdateDTO.CoverPictureId = pictureFile.Id;
                }
            }
        }

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

        var userRole = await _userManager.GetRolesAsync(user);

        if (!userRole.Contains(AppConstant.AdminUserRole))
            if (tournamentExist.TournamentUsers.All(ut => ut.CreatorId != user.Id))
                return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        await _repositoryServiceManager.TournamentService.DeleteTournament(id, user);

        return StatusCode(StatusCodes.Status204NoContent, new ApiResponse<string>("Tournament Successfully Deleted"));
    }

    [HttpPost("{id}/join")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
    public async Task<IActionResult> JoinTournament([FromRoute] Guid id, [FromBody] JoinTournamentDTO? model = null, CancellationToken cancellationToken = default)
    {
        var user = await GetUser();
        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));

        var tournamentExist = await _repositoryServiceManager.TournamentService.GetTournamentById(id);
        if (tournamentExist == null)
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>(404, "Tournament not found"));

        if (tournamentExist.TournamentUsers.Any(ut => ut.UserId == user.Id))
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Your are already in this tournament"));

        if(tournamentExist.AvailableSlot < tournamentExist.TournamentUsers.Count())
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Tournament is full"));

        if (tournamentExist.EntryFee > 0)
        {
            var userWallet = await _repositoryServiceManager.PaymentService.GetUserBalance(user.Id);
            if (userWallet > tournamentExist.EntryFee)
            {
                if (model is not null && string.IsNullOrWhiteSpace(model.Reference))
                {
                    var transactionStatus = await ValidateAndVerifyTransactionReference(model.Reference, cancellationToken);
                    if (transactionStatus.StatusCode != StatusCodes.Status200OK)
                        return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(transactionStatus.StatusCode, transactionStatus.Message));
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "You don't have enough balance to join this tournament"));
                }
            }
        }

        await _repositoryServiceManager.TournamentService.JoinTournamentWithTransactionReference(id, user, model?.Reference, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, new ApiResponse<string>("Tournament Successfully Joined"));
    }

    [HttpGet("featured")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TournamentDTO>>), StatusCodes.Status200OK)]
    public IActionResult GetFeaturedTournaments([FromQuery] int take = 10)
    {
        var tournaments = _repositoryServiceManager.TournamentService.GetFeaturedTournaments();
        tournaments = tournaments.Take(take).OrderBy(x => x.Name).OrderByDescending(t => t.Name);
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<TournamentDTO>>(tournaments));
    }

    [HttpGet("categories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TournamentCategoryDTO>>), StatusCodes.Status200OK)]
    public IActionResult GetTournamentCategories()
    {
        var categories = _repositoryServiceManager.TournamentCategoryService.GetAllCategories();
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<TournamentCategoryDTO>>(categories));
    }
}
