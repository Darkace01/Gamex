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
        if (model is null) return BadRequest(new ApiResponse<UserProfileDTO>(400, "Invalid model object"));

        var user = await GetUser();

        if (user == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<UserProfileDTO>(401, "Unauthorized"));

        if (!string.IsNullOrWhiteSpace(user.Picture?.PublicId))
        {
            var deleteResult = _repo.FileStorageService.DeleteFile(user.Picture.PublicId);
        }

        if (string.IsNullOrWhiteSpace(user.Picture?.FileUrl))
        {
            var uploadResult = await _repo.FileStorageService.SaveFile(model.ProfilePicture, "profile-picture");
            if (uploadResult is not null)
            {
                var updatePiture = await _repo.PictureService.UpdatePicture(new PictureUpdateDTO(user.PictureId, uploadResult.FileUrl, uploadResult.PublicId));
            }
        }
        else
        {
            var uploadResult = await _repo.FileStorageService.SaveFile(model.ProfilePicture, "profile-picture");
            var savePicture = await _repo.PictureService.CreatePicture(new PictureCreateDTO(uploadResult.FileUrl,uploadResult.PublicId));

            user.PictureId = savePicture.Id;
        }
        var existingDisplayName = await _userManager.Users.FirstOrDefaultAsync(u => u.DisplayName.CompareTo(model.DisplayName) == 0);
        if (existingDisplayName != null)
        {
            return BadRequest(new ApiResponse<UserProfileDTO>(400, "Display name already exists"));
        }

        var existingPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber.CompareTo(model.PhoneNumber) == 0);

        if (existingPhoneNumber != null)
        {
            return BadRequest(new ApiResponse<UserProfileDTO>(400, "Phone number already exists"));
        }

        user.DisplayName = model.DisplayName;
        user.PhoneNumber = model.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<UserProfileDTO>(500, "Internal server error"));
        }

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<UserProfileDTO>(new UserProfileDTO()
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            ProfilePictureUrl = user.Picture?.FileUrl ?? string.Empty,
            ProfilePicturePublicId = user.Picture?.PublicId ?? string.Empty
        }));
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
