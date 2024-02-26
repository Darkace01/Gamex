using Paystack.Net.SDK.Models;

namespace Gamex.Service.Contract
{
    public interface IPaystackPayment
    {
        Task<TransactionResponseModel?> VerifyTransaction(string reference);
    }
}