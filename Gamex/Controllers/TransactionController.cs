namespace Gamex.Controllers;
[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/transactions")]
[ApiController]
public class TransactionController(IRepositoryServiceManager repositoryServiceManager, UserManager<ApplicationUser> userManager) : BaseController(userManager, repositoryServiceManager)
{

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<PaymentTransactionDTO>), StatusCodes.Status201Created)]
    public async Task<IActionResult> InitializeTransaction([FromBody] PaymentTransactionCreateDTO paymentTransactionCreateDTO)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));
        }
        if (!user.EmailConfirmed)
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Please confirm your email address to join the tournament"));
        if (paymentTransactionCreateDTO.Amount <= 0)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid amount"));
        }

        var transactionReference = CommonHelpers.GenerateRandomString(20);
        paymentTransactionCreateDTO.TransactionReference = transactionReference;
        paymentTransactionCreateDTO.UserId = user.Id;
        paymentTransactionCreateDTO.Status = Common.TransactionStatus.Pending;
        if (paymentTransactionCreateDTO.TournamentId.Equals(Guid.Empty))
        {
            paymentTransactionCreateDTO.TournamentId = null;
        }
        if (paymentTransactionCreateDTO.TournamentId is not null)
        {
            var tournament = await _repositoryServiceManager.TournamentService.GetTournamentById((Guid)paymentTransactionCreateDTO.TournamentId);
            if (tournament is null) return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(StatusCodes.Status400BadRequest, "Invalid tournament"));
        }

        await _repositoryServiceManager.PaymentService.CreatePaymentTransaction(paymentTransactionCreateDTO);

        var paymentTransaction = await _repositoryServiceManager.PaymentService.GetPaymentTransactionsByReference(transactionReference);

        if (paymentTransaction is null) return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>(500, "Internal Server Error"));

        return StatusCode(StatusCodes.Status201Created, new ApiResponse<PaymentTransactionDTO>(paymentTransaction, 201, "Transaction created"));
    }

    [HttpPost("{transactionReference}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<PaymentTransactionDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateTransactionStatus(string transactionReference, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));
        }

        var transactionStatus = await ValidateAndVerifyTransactionReference(transactionReference, cancellationToken);
        if (transactionStatus.StatusCode != 200)
        {
            return StatusCode(transactionStatus.StatusCode, new ApiResponse<string>(transactionStatus.StatusCode, transactionStatus.Message));
        }
        await _repositoryServiceManager.PaymentService.UpdatePaymentTransactionStatus(transactionReference, Common.TransactionStatus.Success);

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<PaymentTransactionDTO>(transactionStatus.Data, 200, "Wallet has been successfuly funded."));
    }

}
