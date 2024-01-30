using Serilog;

namespace Gamex.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// Configure CORS
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOrigins = configuration["CorsOrigins"];
        string[] originList = [];
        originList = string.IsNullOrEmpty(corsOrigins) ? (["http://localhost:3000", "https://localhost:3000"]) : corsOrigins.Split(";");

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                           builder.WithOrigins(originList)
                                  .AllowAnyMethod()
                                  .AllowAnyHeader()
                                  .AllowCredentials());
        });
    }

    // Rest of the code...
}
