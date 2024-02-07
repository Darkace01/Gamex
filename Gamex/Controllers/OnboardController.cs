using Microsoft.AspNetCore.Identity;

namespace Gamex.Controllers;
[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/onboard")]
[ApiController]
public class OnboardController : ControllerBase
{
    private readonly IRepositoryServiceManager _repo;
    private readonly UserManager<ApplicationUser> _userManager;

    public OnboardController(IRepositoryServiceManager repo, UserManager<ApplicationUser> userManager)
    {
        _repo = repo;
        _userManager = userManager;
    }

    [HttpPost]
    [Authorize]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUserProfile([FromForm] UserProfileUpdateDTO model)
    {
        if (model is null)
        {
            return BadRequest(new ApiResponse<UserProfileDTO>(400, "Invalid model object"));
        }

        var user = GetUser();

        if (user == null)
        {
            return Unauthorized(new ApiResponse<UserProfileDTO>(401, "Unauthorized"));
        }

        if (!string.IsNullOrWhiteSpace(user.Picture?.PublicId))
        {
            await _repo.FileStorageService.DeleteFile(user.Picture.PublicId);
        }

        var existingDisplayName = await _userManager.Users.AsNoTracking().AnyAsync(u => u.DisplayName == model.DisplayName && u.Id != user.Id);
        if (existingDisplayName)
        {
            return BadRequest(new ApiResponse<UserProfileDTO>(400, "Display name already exists"));
        }

        var existingPhoneNumber = await _userManager.Users.AsNoTracking().AnyAsync(u => u.PhoneNumber == model.PhoneNumber && u.Id != user.Id);

        if (existingPhoneNumber)
        {
            return BadRequest(new ApiResponse<UserProfileDTO>(400, "Phone number already exists"));
        }

        var trackedUser = await GetUserWithTracking();

        if (trackedUser is null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<UserProfileDTO>(500, "Internal server error"));
        }

        if (model.ProfilePicture is not null)
        {
            if (!string.IsNullOrWhiteSpace(user.Picture?.FileUrl))
            {
                var uploadResult = await _repo.FileStorageService.SaveFile(model.ProfilePicture, "profile-picture");
                if (uploadResult is not null)
                {
                    await _repo.PictureService.UpdatePicture(new PictureUpdateDTO(user.PictureId, uploadResult.FileUrl, uploadResult.PublicId));
                }
            }
            else
            {
                var uploadResult = await _repo.FileStorageService.SaveFile(model.ProfilePicture, "profile-picture");
                var savePicture = await _repo.PictureService.CreatePicture(new PictureCreateDTO(uploadResult.FileUrl, uploadResult.PublicId));

                trackedUser.PictureId = savePicture.Id;
            }
        }

        trackedUser.DisplayName = model.DisplayName;
        trackedUser.PhoneNumber = model.PhoneNumber;
        trackedUser.FirstName = model.FirstName;
        trackedUser.LastName = model.LastName;

        var result = await _userManager.UpdateAsync(trackedUser);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<UserProfileDTO>(500, "Internal server error"));
        }

        user = GetUser();
        if(user is null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<UserProfileDTO>(500, "Internal server error"));
        }
        return Ok(new ApiResponse<UserProfileDTO>(GetUserProfileDTO(user)));
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDTO>), StatusCodes.Status200OK)]
    public IActionResult GetUserProfile()
    {
        var user = GetUser();

        if (user == null)
        {
            return Unauthorized(new ApiResponse<UserProfileDTO>(401, "Unauthorized"));
        }

        return Ok(new ApiResponse<UserProfileDTO>(GetUserProfileDTO(user)));
    }

    #region Helpers
    private ApplicationUser? GetUser()
    {
        var username = User?.Identity?.Name;
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }
        var user = _repo.ExtendedUserService.GetUserByName(username);
        return user;
    }

    private async Task<ApplicationUser?> GetUserWithTracking()
    {
        var username = User?.Identity?.Name;
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }
        var user = await _userManager.FindByNameAsync(username);
        return user;
    }

    private static UserProfileDTO GetUserProfileDTO(ApplicationUser user)
    {
        return new UserProfileDTO(user.FirstName, user.LastName, user.DisplayName, user.Email, user.PhoneNumber, user.Picture?.FileUrl ?? string.Empty, user.Picture?.PublicId ?? string.Empty);
    }
    #endregion
}
