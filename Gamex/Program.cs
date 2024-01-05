using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Configure Json Serializer
builder.Services.ConfigureJsonSerializer();

builder.Services.AddAuthorization();
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GamexDbContext>(options =>
    options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null
            );
    }));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureCors();

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

//builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
//    .AddEntityFrameworkStores<GamexDbContext>()
//    .AddApiEndpoints();


var app = builder.Build();
app.UseSerilogRequestLogging();
//app.MapIdentityApi<ApplicationUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI();


app.ConfigureExceptionHandler(app.Logger, app.Configuration);

app.UseHealthChecks("/app/health");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
