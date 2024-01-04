using Gamex.Common;
using System.IdentityModel.Tokens.Jwt;

namespace Gamex.Controllers;
[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/auth")]
[ApiController]
public class AuthController(IRepositoryServiceManager repo, UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly IRepositoryServiceManager _repo = repo;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginDTO model, CancellationToken cancellationToken)
    {
        if(model == null) return BadRequest(new ApiResponse<LoginDTO>(400, "Invalid authentication request"));
        
        if(!ModelState.IsValid) return BadRequest(new ApiResponse<LoginDTO>(400, "Invalid authentication request"));

        var user = await _userManager.FindByNameAsync(model.Username);
        user ??= await _userManager.FindByEmailAsync(model.Username);
        if(user == null) return BadRequest(new ApiResponse<LoginDTO>(400, "Invalid username or password."));

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        if(!isPasswordValid) return BadRequest(new ApiResponse<LoginDTO>(400, "Invalid username or password."));

        return Ok(await GenerateLoginTokenandResponseForUser(user));
    }


    private async Task<ApiResponse<LoginResponseDTO>> GenerateLoginTokenandResponseForUser(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var authToken = _repo.JWTHelper.GenerateToken(user, userRoles);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(authToken);

        var refreshToken = SecurityHelpers.GenerateRefreshToken();

        ApiResponse<LoginResponseDTO> loginResponse = new(new LoginResponseDTO(accessToken,refreshToken, authToken.ValidTo.Ticks, "Bearer"));
        return loginResponse;
    }
}
