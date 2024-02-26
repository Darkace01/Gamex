using CloudinaryDotNet.Actions;
using Gamex.Service.Implementation;
using Microsoft.AspNetCore.Identity;

namespace Gamex.Controllers;
[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/transactions")]
[ApiController]
public class TransactionController(IRepositoryServiceManager repositoryServiceManager, UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly IRepositoryServiceManager _repositoryServiceManager = repositoryServiceManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;


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

        if(paymentTransactionCreateDTO.Amount <= 0)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid amount"));
        }

        var transactionReference = CommonHelpers.GenerateRandomString(20);
        paymentTransactionCreateDTO.TransactionReference = transactionReference;
        paymentTransactionCreateDTO.UserId = user.Id;
        paymentTransactionCreateDTO.Status = Common.TransactionStatus.Pending;

        await _repositoryServiceManager.PaymentService.CreatePaymentTransaction(paymentTransactionCreateDTO);

        var paymentTransaction = await _repositoryServiceManager.PaymentService.GetPaymentTransactionsByReference(transactionReference);

        if(paymentTransaction is null) return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>(500, "Internal Server Error"));

        return StatusCode(StatusCodes.Status201Created, new ApiResponse<PaymentTransactionDTO>(paymentTransaction,201, "Transaction created"));
    }

    [HttpPost("{transactionReference}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<PaymentTransactionDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateTransactionStatus(string transactionReference)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(400, "Invalid model object"));

        var user = await GetUser();
        if (user == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>(401, "Unauthorized"));
        }

        var response = await _repositoryServiceManager.PaystackPayment.VerifyTransaction(transactionReference);

        if(response is null) return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(500, "Transaction not found"));

        if (!response.status)
        {
            await _repositoryServiceManager.PaymentService.UpdatePaymentTransactionStatus(transactionReference, Common.TransactionStatus.Failed);
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(StatusCodes.Status400BadRequest, $"Unable to complete payment.->{response.message}"));
        }

        if (!response.data.status.ToLower().Equals("success"))
        {
            await _repositoryServiceManager.PaymentService.UpdatePaymentTransactionStatus(transactionReference, Common.TransactionStatus.Failed);
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(StatusCodes.Status400BadRequest, $"Unable to complete payment.->{response.message}"));
        }

        var amountPaid = (decimal)(response.data.amount / 100); //Always divide returned amount by 100 to get 
        var transactionRef = response.data.reference;

        var paymentTransaction = await _repositoryServiceManager.PaymentService.GetPaymentTransactionsByReference(transactionRef);

        if(paymentTransaction is null) return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(500, "No Associating transaction"));

        if (!transactionRef.Equals(transactionReference)) return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(StatusCodes.Status400BadRequest, "Invalid transaction reference"));

        if (amountPaid == 0 || amountPaid < paymentTransaction.Amount) return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>(StatusCodes.Status400BadRequest, "Invalid transaction :: Amount paid is less than expected amount"));

        await _repositoryServiceManager.PaymentService.UpdatePaymentTransactionStatus(transactionReference, Common.TransactionStatus.Success);

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<PaymentTransactionDTO>(paymentTransaction, 200, "Transaction updated"));
    }

    #region Helpers
    private async Task<ApplicationUser?> GetUser()
    {
        var username = User?.Identity?.Name;
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }
        var user = await _userManager.FindByNameAsync(username);
        return user;
    }
    #endregion
}
