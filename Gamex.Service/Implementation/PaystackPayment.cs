using Gamex.Common;
using Paystack.Net.SDK.Models;
using System.Text.Json;

namespace Gamex.Service.Implementation;

public class PaystackConstant
{
    public const string BASE_URL = "https://api.paystack.co/";

    public const string AUTHORIZATION_TYPE = "Bearer";

    public const string REQUEST_MEDIA_TYPE = "application/json";

}

public class PaystackPayment(IConfiguration configuration) : IPaystackPayment
{
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    /// Verifies transaction by reference number
    /// </summary>
    /// <param name="reference"></param>
    /// <returns>TransactionVerificationResponseModel</returns>

    public async Task<TransactionResponseModel?> VerifyTransaction(string reference)
    {
        var secretKey = _configuration["Paystack:SecretKey"];

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new ArgumentNullException("Paystack Secret key is required");

        var client = HttpFactory.InitHttpClient(PaystackConstant.BASE_URL)
                  .AddAuthorizationHeader(PaystackConstant.AUTHORIZATION_TYPE, secretKey)
                  .AddMediaType(PaystackConstant.REQUEST_MEDIA_TYPE)
                  .AddHeader("cache-control", "no-cache");

        var response = await client.GetAsync($"transaction/verify/{reference}");

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TransactionResponseModel?>(json);
    }
}
