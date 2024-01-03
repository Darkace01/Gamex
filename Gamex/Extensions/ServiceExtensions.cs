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

    /// <summary>
    /// Configure Swagger
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Gamex API",
                Version = "v1",
                Description = "Gamex API",
                Contact = new OpenApiContact
                {
                    Name = "Gamex",
                    Email = ""
                }

            });
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                              "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                              "Example: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            swagger.OperationFilter<SwaggerFileOperationFilter>();
        });
    }
}
