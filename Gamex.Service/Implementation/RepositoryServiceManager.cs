namespace Gamex.Service.Implementation;

public class RepositoryServiceManager(GamexDbContext context) : IRepositoryServiceManager
{
    private readonly GamexDbContext _context = context;

    private ITournamentService _tournamentService;

    public ITournamentService TournamentService
    {
        get
        {
            _tournamentService ??= new TournamentService(_context);
            return _tournamentService;
        }
    }
}
