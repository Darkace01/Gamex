using Gamex.Data.Interceptors;
using Sentry.Profiling;
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
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                           builder.AllowAnyMethod()
                                  .AllowAnyHeader()
                                  .AllowAnyOrigin()
                                  //.WithOrigins("*")
                                  .SetIsOriginAllowed(origin => true));
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
        services.AddScoped<IRepositoryServiceManager, RepositoryServiceManager>();
        services.AddScoped<ISMTPMailService, SMTPMailService>();
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

    /// <summary>
    /// Configure Serilog logger
    /// </summary>
    /// <param name="hostBuilder"></param>
    public static void ConfigureSerilog(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
    }

    /// <summary>
    /// Configure JWT Authentication
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void ConfigureAuthenticationWithJWT(this IServiceCollection services, IConfiguration configuration) => services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        // Adding Jwt Bearer  
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? throw new ArgumentNullException("JWT:Secret")))
            };
        });

    /// <summary>
    /// Configure Identity
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureIdentity(this IServiceCollection services) => services.AddIdentity<ApplicationUser, IdentityRole>(
                                options =>
                                {
                                    options.User.RequireUniqueEmail = true;
                                    options.Password.RequiredLength = 6;
                                })
                                .AddEntityFrameworkStores<GamexDbContext>()
                                .AddDefaultTokenProviders();

    /// <summary>
    /// Configure External Authentication. Like google, facebook, etc.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void ConfigureExternalAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication().AddGoogleOpenIdConnect(googleOptions =>
        {
            googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
        });
    }

    /// <summary>
    /// Configure Interceptors
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureInterceptors(this IServiceCollection services)
    {
        services.AddSingleton<SoftDeleteInterceptor>();
    }

    /// <summary>
    /// Configure Database
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<GamexDbContext>((sp, options) =>
            options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null
                    );
            })
            .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>())
            );
    }

    /// <summary>
    /// Configure Sentry
    /// </summary>
    /// <param name="builderContext"></param>
    public static void ConfigureSentry(this IWebHostBuilder builderContext)
    {
        builderContext.UseSentry();
    }

    /// <summary>
    /// Configure Caching
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void ConfigureCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddResponseCaching();
        var config = configuration.GetConnectionString("RedisConnection");
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "Gamex_";
        });
    }

    /// <summary>
    /// Add Rate Limiting
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    public static void AddRateLimiting(this IServiceCollection services, IConfiguration config)
    {
        var myOptions = new RateLimitOptions();
        config.GetSection(RateLimitOptions.MyRateLimit).Bind(myOptions);
        services.AddRateLimiter(_ => _
        .AddFixedWindowLimiter(policyName: AppConstant.RateLimiting.FixedPolicy, options =>
        {
            options.PermitLimit = myOptions.PermitLimit;
            options.Window = TimeSpan.FromSeconds(myOptions.Window);
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.QueueLimit = myOptions.QueueLimit;
            options.AutoReplenishment = myOptions.AutoReplenishment;
        }));
    }
}
