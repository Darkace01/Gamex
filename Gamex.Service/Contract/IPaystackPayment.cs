using Paystack.Net.SDK.Models;

namespace Gamex.Service.Contract
{
    public interface IPaystackPayment
    {
        /// <summary>
        /// Verifies a transaction using the specified reference.
        /// </summary>
        /// <param name="reference">The reference of the transaction to verify.</param>
        /// <returns>A <see cref="TransactionResponseModel"/> representing the result of the verification, or null if the verification fails.</returns>
        Task<TransactionResponseModel?> VerifyTransaction(string reference);
    }
}