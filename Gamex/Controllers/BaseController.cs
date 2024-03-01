using Microsoft.AspNetCore.Identity;

namespace Gamex.Controllers;
public class BaseController(UserManager<ApplicationUser> userManager, IRepositoryServiceManager repositoryServiceManager) : ControllerBase
{
    protected readonly UserManager<ApplicationUser> _userManager = userManager;
    protected readonly IRepositoryServiceManager _repositoryServiceManager = repositoryServiceManager;
    #region Helpers
    internal async Task<ApplicationUser?> GetUser()
    {
        var username = User?.Identity?.Name;
        if (username is null)
            return null;
        var user = await _userManager.FindByNameAsync(username);
        return user;
    }

    internal async Task<ApiResponse<PaymentTransactionDTO>> ValidateAndVerifyTransactionReference(string transactionReference, CancellationToken cancellationToken = default)
    {
        var response = await _repositoryServiceManager.PaystackPayment.VerifyTransaction(transactionReference);

        if (response is null) return new ApiResponse<PaymentTransactionDTO>(StatusCodes.Status500InternalServerError, "Transaction not found");

        if (!response.status)
        {
            await _repositoryServiceManager.PaymentService.UpdatePaymentTransactionStatus(transactionReference, Common.TransactionStatus.Failed, cancellationToken);
            return new ApiResponse<PaymentTransactionDTO>(StatusCodes.Status400BadRequest, $"Unable to complete payment.->{response.message}");
        }

        if (!response.data.status.ToLower().Equals("success"))
        {
            await _repositoryServiceManager.PaymentService.UpdatePaymentTransactionStatus(transactionReference, Common.TransactionStatus.Failed, cancellationToken);
            return new ApiResponse<PaymentTransactionDTO>(StatusCodes.Status400BadRequest, $"Unable to complete payment.->{response.message}");
        }

        var amountPaid = (decimal)(response.data.amount / 100); //Always divide returned amount by 100 to get 
        var transactionRef = response.data.reference;

        var paymentTransaction = await _repositoryServiceManager.PaymentService.GetPaymentTransactionsByReference(transactionRef, cancellationToken);

        if (paymentTransaction is null) return new ApiResponse<PaymentTransactionDTO>(500, "No Associating transaction");

        if (!transactionRef.Equals(transactionReference)) return new ApiResponse<PaymentTransactionDTO>(StatusCodes.Status400BadRequest, "Invalid transaction reference");

        if (amountPaid == 0 || amountPaid < paymentTransaction.Amount) return new ApiResponse<PaymentTransactionDTO>(StatusCodes.Status400BadRequest, "Invalid transaction :: Amount paid is less than expected amount");

        return new ApiResponse<PaymentTransactionDTO>(StatusCodes.Status200OK, "Transaction verified successfully");

    }
    #endregion
}
