using Gamex.Service.Contract;
using Gamex.Service.Implementation;

namespace GamexAdmin.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureRepository(this IServiceCollection services)
    {
        services.AddScoped<ITournamentService, TournamentService>();
        services.AddSingleton<ISMTPMailService, SMTPMailService>();
        //services.AddScoped<IFileStorageService, FileStorageService>();
        //services.AddScoped<IPictureService, PictureService>();
        services.AddScoped<IRepositoryServiceManager, RepositoryServiceManager>();
    }
}
