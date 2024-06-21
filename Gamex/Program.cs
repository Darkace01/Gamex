using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRateLimiting(builder.Configuration);
builder.Services.AddAuthorization();
// Add services to the container.
builder.Services.ConfigureInterceptors();
builder.Services.ConfigureDatabase(builder.Configuration);

// Add Opentelementry
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resouce => resouce.AddService("GamexAPI"))
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();
        metrics
            .AddOtlpExporter(options => options.Endpoint = new Uri("http://aspire-dashboard:18889"));
    })
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation();

        tracing
            .AddOtlpExporter();
    });
builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureCors(builder.Configuration);

//Identity
builder.Services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
builder.Services.ConfigureIdentity();

// Caching setup
builder.Services.ConfigureCaching(builder.Configuration);

// Dependency Injection for repositories
builder.Services.ConfigureRepository();

//JWT
builder.Services.ConfigureAuthenticationWithJWT(builder.Configuration);

builder.Services.ConfigureSwagger();
builder.Services.ConfigureVersioning();

builder.Services.AddHealthChecks();

//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.WebHost.ConfigureSentry();
var app = builder.Build();
app.UseSerilogRequestLogging();
// Configure the HTTP request pipeline.

app.UseResponseCaching();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");

app.ConfigureExceptionHandler(app.Logger, app.Configuration);

app.UseHealthChecks("/app/health");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
