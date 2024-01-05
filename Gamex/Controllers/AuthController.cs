namespace Gamex.Controllers;
[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/auth")]
[ApiController]
public class AuthController(IRepositoryServiceManager repo, UserManager<ApplicationUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager) : ControllerBase
{
    private readonly IRepositoryServiceManager _repo = repo;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginDTO model, CancellationToken cancellationToken)
    {
        if (model == null) return BadRequest(new ApiResponse<LoginDTO>(400, "Invalid authentication request"));

        if (!ModelState.IsValid) return BadRequest(new ApiResponse<LoginDTO>(400, "Invalid authentication request"));

        var user = await _userManager.FindByNameAsync(model.Username);
        user ??= await _userManager.FindByEmailAsync(model.Username);
        if (user == null) return BadRequest(new ApiResponse<LoginDTO>(400, "Invalid username or password."));

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!isPasswordValid) return BadRequest(new ApiResponse<LoginDTO>(400, "Invalid username or password."));

        return Ok(await GenerateLoginTokenandResponseForUser(user));
    }

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO model)
    {
        if (model == null) return BadRequest(new ApiResponse<LoginResponseDTO>(400, "Invalid refresh token request"));

        if (!ModelState.IsValid) return BadRequest(new ApiResponse<LoginResponseDTO>(400, "Invalid refresh token request"));

        var principal = _repo.JWTHelper.GetPrincipalFromExpiredToken(model.AccessToken);

        if (principal == null) return BadRequest(new ApiResponse<LoginResponseDTO>(400, "Invalid refresh token request"));

        var username = principal.Identity?.Name;

        if (string.IsNullOrWhiteSpace(username)) return BadRequest(new ApiResponse<LoginResponseDTO>(400, "Invalid refresh token request"));

        var user = await _userManager.FindByNameAsync(username);

        if (user == null) return BadRequest(new ApiResponse<LoginResponseDTO>(400, "Invalid refresh token request"));

        return Ok(await GenerateLoginTokenandResponseForUser(user));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
        if (model == null) return BadRequest(new ApiResponse<string>(400, "Invalid registration request"));

        if (!ModelState.IsValid) return BadRequest(new ApiResponse<string>(400, "Invalid registration request"));

        var userExists = await _userManager.FindByNameAsync(model.Username);
        userExists ??= await _userManager.FindByEmailAsync(model.Email);

        if (userExists != null) return BadRequest(new ApiResponse<string>(400, "User already exists!"));

        ApplicationUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username,
            DisplayName = model.DisplayName,
            PhoneNumber = model.PhoneNumber
        };

        if (!await _roleManager.RoleExistsAsync(AppConstant.PublicUserRole))
            await _roleManager.CreateAsync(new IdentityRole(AppConstant.PublicUserRole));

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded) return BadRequest(new ApiResponse<string>(400, $"User creation failed! Please check user details and try again. {result?.Errors?.FirstOrDefault()?.Description}"));

        await _userManager.AddToRoleAsync(user, AppConstant.PublicUserRole);

        return Ok(new ApiResponse<string>(200, "User created successfully!"));
    }

    [HttpPost("external/google")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExternalRegisterOrLoginWithGoogle([FromBody] ExternalAuthDTO model)
    {
        if(model is null) return BadRequest(new ApiResponse<LoginResponseDTO>(400, "Invalid external authentication request"));

        if (!ModelState.IsValid) return BadRequest(new ApiResponse<LoginResponseDTO>(400, "Invalid external authentication request"));

        var response = await ValidateUserTokenForGoogle(model.Token);

        if (response.HasError) return BadRequest(new ApiResponse<LoginResponseDTO>(400, response.Message));

        var userExist = await _userManager.FindByEmailAsync(response.Data?.Email);
        userExist ??= await _userManager.FindByNameAsync(response.Data?.Email);

        if(userExist is not null)
        {
            // Return a token for the user
            if (userExist.ExternalAuthInWithGoogle == false)
            {
                userExist.ExternalAuthInWithGoogle = true;
                userExist.EmailConfirmed = true;
                await _userManager.UpdateAsync(userExist);
            }

            return Ok(await GenerateLoginTokenandResponseForUser(userExist));
        }

        var picture = await _repo.PictureService.CreatePicture(new PictureCreateDTO()
        {
            FileUrl = response.Data?.Picture
        });

        // Create a new user
        ApplicationUser user = new()
        {
            Email = response?.Data?.Email,
            EmailConfirmed = true,
            //FirstName = response.data.GivenName,
            //LastName = response.data.FamilyName,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = response?.Data?.Email,
            ExternalAuthInWithGoogle = true,
            //TODO: Save profile picture url for user
            PictureId = picture.Id,
            //ReceivePushNotification = true
        };

        if (!await _roleManager.RoleExistsAsync(AppConstant.PublicUserRole))
        {
            await _roleManager.CreateAsync(new IdentityRole(AppConstant.PublicUserRole));
        }

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded) return BadRequest(new ApiResponse<string>(400, $"User creation failed! Please check user details and try again. {result?.Errors?.FirstOrDefault()?.Description}"));

        await _userManager.AddToRoleAsync(user, AppConstant.PublicUserRole);

        return Ok(await GenerateLoginTokenandResponseForUser(user));

    }

    #region Private Methods
    private async Task<ApiResponse<GoogleJsonWebSignature.Payload>> ValidateUserTokenForGoogle(string token)
    {
        var mobileClientId = _configuration["Authentication:Google:MobileClientId"];
        var webClientId = _configuration["Authentication:Google:ClientId"];
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            ExpirationTimeClockTolerance = TimeSpan.FromHours(1)
        };
        GoogleJsonWebSignature.Payload payload = null;
        bool isValidToken = false;
        var message = string.Empty;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
            if (payload != null && ((string?)payload?.Audience == mobileClientId || (string?)payload?.Audience == webClientId))
            {
                isValidToken = true;
            }
        }
        catch (Exception ex)
        {
            message = ex.Message;
        }

        var response = new ApiResponse<GoogleJsonWebSignature.Payload>()
        {
            HasError = !isValidToken,
            Data = payload,
            Message = message
        };
        return response;
    }
    private async Task<ApiResponse<LoginResponseDTO>> GenerateLoginTokenandResponseForUser(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var authToken = _repo.JWTHelper.GenerateToken(user, userRoles);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(authToken);

        var refreshToken = SecurityHelpers.GenerateRefreshToken();

        _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

        await _userManager.UpdateAsync(user);

        ApiResponse<LoginResponseDTO> loginResponse = new(new LoginResponseDTO(accessToken, refreshToken, authToken.ValidTo.Ticks, "Bearer"));
        return loginResponse;
    }
    #endregion
}
