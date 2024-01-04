namespace Gamex.Service.Implementation;

public class RepositoryServiceManager(GamexDbContext context, IConfiguration configuration) : IRepositoryServiceManager
{
    private readonly GamexDbContext _context = context;
    private readonly IConfiguration _configuration = configuration;

    private ITournamentService _tournamentService;
    private IJWTHelper _jwtHelper;

    public ITournamentService TournamentService
    {
        get
        {
            _tournamentService ??= new TournamentService(_context);
            return _tournamentService;
        }
    }

    public IJWTHelper JWTHelper
    {
        get
        {
            _jwtHelper ??= new JWTHelper(_configuration);
            return _jwtHelper;
        }
    }
}
