using Gamex.Common;

namespace Gamex.Controllers;
[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/auth")]
[ApiController]
public class AuthController(IRepositoryServiceManager repo, UserManager<ApplicationUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, ISMTPMailService mailService) : BaseController(userManager, repo)
{
    private const string InvalidAuthenticationRequest = "Invalid authentication request";
    private const string InvalidUsernameAndPassword = "Invalid username or password.";
    private const string InvalidConfirmMailRequest = "Invalid email confirmation request";
    private readonly IConfiguration _configuration = configuration;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly ISMTPMailService _emailService = mailService;

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginDTO model, CancellationToken cancellationToken)
    {
        if (model == null) return BadRequest(new ApiResponse<LoginDTO>(400, InvalidAuthenticationRequest));

        if (!ModelState.IsValid) return BadRequest(new ApiResponse<LoginDTO>(400, InvalidAuthenticationRequest));

        var user = await _userManager.FindByNameAsync(model.Username);
        user ??= await _userManager.FindByEmailAsync(model.Username);
        if (user == null) return BadRequest(new ApiResponse<LoginDTO>(400, InvalidUsernameAndPassword));

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!isPasswordValid) return BadRequest(new ApiResponse<LoginDTO>(400, InvalidUsernameAndPassword));

        var isUserActive = user.LockoutEnabled;
        if (!isUserActive) return BadRequest(new ApiResponse<LoginDTO>(400, "User is not active. Please contact administrator."));

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

        var principal = _repositoryServiceManager.JWTHelper.GetPrincipalFromExpiredToken(model.AccessToken);

        if (principal == null) return BadRequest(new ApiResponse<LoginResponseDTO>(400, "Invalid refresh token request"));

        var username = principal.Identity?.Name;

        if (string.IsNullOrWhiteSpace(username)) return BadRequest(new ApiResponse<LoginResponseDTO>(400, "Invalid refresh token request"));

        var user = await _userManager.FindByNameAsync(username);

        if (user == null) return BadRequest(new ApiResponse<LoginResponseDTO>(400, "Invalid refresh token request"));

        if (user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now) return BadRequest(new ApiResponse<LoginResponseDTO>(400, "Invalid refresh token request"));

        var isUserActive = user.LockoutEnabled;
        if (!isUserActive) return BadRequest(new ApiResponse<LoginDTO>(400, "User is not active. Please contact administrator."));

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

        if (!model.DisplayName.Trim().HasNoSpecialCharacters()) return BadRequest(new ApiResponse<string>(400, "Invalid display name!, display name should not contain special characters."));
        if (!model.Username.Trim().HasNoSpecialCharacters()) return BadRequest(new ApiResponse<string>(400, "Invalid username!, username should not contain special characters."));


        ApplicationUser user = new()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username,
            DisplayName = model.DisplayName,
            PhoneNumber = model.PhoneNumber
        };

        if (!await _roleManager.RoleExistsAsync(AppConstant.PublicUserRole))
            await _roleManager.CreateAsync(new IdentityRole(AppConstant.PublicUserRole));

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded) return BadRequest(new ApiResponse<string>(400, $"User creation failed! Please check user details and try again. {result.Errors?.FirstOrDefault()?.Description}"));

        await _userManager.AddToRoleAsync(user, AppConstant.PublicUserRole);

        var token = await _repositoryServiceManager.ExtendedUserService.GenerateUserConfirmationCode(user.Id);

        var message = $"Please find your confirmation code : {token.Code}";

        _ = await _emailService.SendEmailAsync(model.Email, "Confirm Email", message);

        return Ok(new ApiResponse<string>("", 200, "User created successfully!"));
    }

    [HttpPost("external/google")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExternalRegisterOrLoginWithGoogle([FromBody] ExternalAuthDTO model)
    {
        if (model is null) return BadRequest(new ApiResponse<LoginResponseDTO>(400, "Invalid external authentication request"));

        if (!ModelState.IsValid) return BadRequest(new ApiResponse<LoginResponseDTO>(400, "Invalid external authentication request"));

        var response = await ValidateUserTokenForGoogle(model.Token);

        if (response.HasError) return BadRequest(new ApiResponse<LoginResponseDTO>(400, response.Message));

        var userExist = await _userManager.FindByEmailAsync(response.Data.Email);
        userExist ??= await _userManager.FindByNameAsync(response.Data.Email);

        if (userExist is not null)
        {
            // Return a token for the user
            if (userExist.ExternalAuthInWithGoogle)
            {
                userExist.ExternalAuthInWithGoogle = true;
                await _userManager.UpdateAsync(userExist);
            }

            var isUserActive = userExist.LockoutEnabled;
            if (!isUserActive) return BadRequest(new ApiResponse<LoginDTO>(400, "User is not active. Please contact administrator."));

            return Ok(await GenerateLoginTokenandResponseForUser(userExist));
        }

        var picture = await _repositoryServiceManager.PictureService.CreatePicture(new PictureCreateDTO(response.Data?.Picture, ""));

        // Create a new user
        ApplicationUser user = new()
        {
            Email = response.Data?.Email,
            FirstName = response.Data?.GivenName ?? string.Empty,
            LastName = response.Data?.FamilyName ?? string.Empty,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = response.Data?.Email,
            ExternalAuthInWithGoogle = true,
            PictureId = picture.Id,
            DisplayName = response.Data?.Name,
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

    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
    {
        if (model is null) return BadRequest(new ApiResponse<string>(400, "Invalid change password request"));

        if (!ModelState.IsValid) return BadRequest(new ApiResponse<string>(400, "Invalid change password request"));

        var userName = User.Identity?.Name;

        if (string.IsNullOrWhiteSpace(userName)) return BadRequest(new ApiResponse<string>(400, "Invalid change password request"));

        var user = await _userManager.FindByNameAsync(userName);

        if (user == null) return BadRequest(new ApiResponse<string>(400, "Invalid change password request"));

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (!result.Succeeded) return BadRequest(new ApiResponse<string>(400, "Invalid change password request"));

        return Ok(new ApiResponse<string>("", 200, "Password changed successfully!"));
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
    {
        if (model is null) return BadRequest(new ApiResponse<string>(400, "Invalid forgot password request"));

        if (!ModelState.IsValid) return BadRequest(new ApiResponse<string>(400, "Invalid forgot password request"));

        var responseMessage = "If the email is registered, a password reset link will be sent to the email address provided.";

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null) return BadRequest(new ApiResponse<string>(400, responseMessage));

        var token = await _repositoryServiceManager.ExtendedUserService.GenerateUserConfirmationCode(user.Id);

        var message = $"Please find your password resent code : {token.Code}";

        // Send email with password reset link
        _ = await _emailService.SendEmailAsync(model.Email, "Reset Password", message);

        return Ok(new ApiResponse<string>("", 200, responseMessage));
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
    {
        if (model is null) return BadRequest(new ApiResponse<string>(400, "Invalid reset password request"));

        if (!ModelState.IsValid) return BadRequest(new ApiResponse<string>(400, "Invalid reset password request"));

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null) return BadRequest(new ApiResponse<string>(400, "Invalid reset password request"));

        var isValidCode = await _repositoryServiceManager.ExtendedUserService.VerifyUserConfirmationCode(user.Id, model.Code);

        if (!isValidCode) return BadRequest(new ApiResponse<string>(400, "Invalid reset password request"));

        var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, passwordResetToken, model.NewPassword);

        if (!result.Succeeded) return BadRequest(new ApiResponse<string>(400, "Invalid reset password request"));

        return Ok(new ApiResponse<string>("", 200, "Password reset successfully!"));
    }

    [HttpPost("send-confirmation-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SendConfirmationEmail([FromBody] RequestConfirmEmailDTO model)
    {
        if (string.IsNullOrWhiteSpace(model.Email)) return BadRequest(new ApiResponse<string>(400, "Invalid email address"));

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null) return BadRequest(new ApiResponse<string>(400, "Invalid email address"));

        if (user.EmailConfirmed) return BadRequest(new ApiResponse<string>(400, "Email already confirmed!"));

        var token = await _repositoryServiceManager.ExtendedUserService.GenerateUserConfirmationCode(user.Id);

        var message = $"Please find your confirmation code : {token.Code}";

        _ = await _emailService.SendEmailAsync(model.Email, "Confirm Email", message);

        return Ok(new ApiResponse<string>("", 200, "Confirmation email sent successfully!"));
    }

    [HttpPost("confirm-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDTO model)
    {
        if (model is null) return BadRequest(new ApiResponse<string>(400, InvalidConfirmMailRequest));

        if (!ModelState.IsValid) return BadRequest(new ApiResponse<string>(400, InvalidConfirmMailRequest));

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null) return BadRequest(new ApiResponse<string>(400, InvalidConfirmMailRequest));

        var isValidCode = await _repositoryServiceManager.ExtendedUserService.VerifyUserConfirmationCode(user.Id, model.Code);

        if (!isValidCode) return BadRequest(new ApiResponse<string>(400, InvalidConfirmMailRequest));

        user.EmailConfirmed = true;

        await _userManager.UpdateAsync(user);

        return Ok(new ApiResponse<string>("", 200, "Email confirmed successfully!"));
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
        GoogleJsonWebSignature.Payload? payload = null;
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
            Data = payload!,
            Message = message
        };
        return response;
    }
    private async Task<ApiResponse<LoginResponseDTO>> GenerateLoginTokenandResponseForUser(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var authToken = _repositoryServiceManager.JWTHelper.GenerateToken(user, userRoles);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(authToken);

        var refreshToken = SecurityHelpers.GenerateRefreshToken();

        _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

        await _userManager.UpdateAsync(user);

        ApiResponse<LoginResponseDTO> loginResponse = new(new LoginResponseDTO(user.Id, accessToken, refreshToken, authToken.ValidTo.Ticks, "Bearer", new UserMiniDTO(user.Email, user.Picture?.FileUrl, user.FirstName, user.LastName)));
        return loginResponse;
    }
    #endregion
}
