namespace Gamex.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// Configure CORS
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                           builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader()
                                  .AllowCredentials());
        });
    }

    /// <summary>
    /// Configure API Versioning
    /// </summary>
    /// <param name="services"></param>
    
    public static void ConfigureVersioning(this IServiceCollection services) => services.AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
    });

    /// <summary>
    /// Configure Dependency Injection
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureRepository(this IServiceCollection services)
    {
    }
}
