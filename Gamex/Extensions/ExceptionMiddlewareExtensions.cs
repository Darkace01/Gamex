namespace Gamex.Extensions;

public static class ExceptionMiddlewareExtensions
{
    /// <summary>
    /// Configure exception handler
    /// </summary>
    /// <param name="app"></param>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger, IConfiguration configuration)
    {
        _ = app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                var loggedInUser = string.IsNullOrWhiteSpace(context?.Request?.HttpContext?.User?.Identity?.Name) ? "Anonymous" : context.Request.HttpContext.User.Identity.Name;
                if (contextFeature != null)
                {
                    logger.LogError(contextFeature.Error, $"{contextFeature.Endpoint} Something went wrong: {contextFeature.Error}");
                    // check if the app is in development mode
                    var devMode = configuration.GetValue<bool?>("DevMode");
                    var response = JsonSerializer.Serialize(new ApiResponse<string>()
                    {
                        StatusCode = context?.Response.StatusCode ?? 500,
                        Message = devMode == true ? $"Internal Server Error: {contextFeature.Error}" : $"Internal Server Error",
                        HasError = true,
                    }, //serialize the response object to camelCase
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    await context?.Response?.WriteAsync(response);
                }
            });
        });
    }
}
