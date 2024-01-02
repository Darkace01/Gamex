﻿using Gamex.DTO;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

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
                var config = context.Features.Get<IConfiguration>();
                var loggedInUser = string.IsNullOrWhiteSpace(context?.Request?.HttpContext?.User?.Identity?.Name) ? "Anonymous" : context.Request.HttpContext.User.Identity.Name;
                if (contextFeature != null)
                {
                    logger.LogError(contextFeature.Error, $"{contextFeature.Endpoint} Something went wrong: {contextFeature.Error}");
                    var response = JsonSerializer.Serialize(new ApiResponse<string>()
                    {
                        StatusCode = context?.Response.StatusCode ?? 500,
                        Message = $"Internal Server Error: {contextFeature.Error}",
                        HasError = true,
                    });
                    await context?.Response?.WriteAsync(response);
                }
            });
        });
    }
}
