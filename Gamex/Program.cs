using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
// Add services to the container.
builder.Services.ConfigureInterceptors();
builder.Services.ConfigureDatabase(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureCors(builder.Configuration);

//Identity
builder.Services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
builder.Services.ConfigureIdentity();

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
