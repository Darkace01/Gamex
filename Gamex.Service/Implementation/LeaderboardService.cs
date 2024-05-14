namespace Gamex.Service.Implementation;

public class LeaderboardService(GamexDbContext context) : ILeaderboardService
{
    private readonly GamexDbContext _context = context;

    /// <summary>
    /// Retrieves the leaderboard with player rankings based on points and tournaments.
    /// </summary>
    /// <returns>An enumerable collection of LeaderboardDTO objects representing the leaderboard.</returns>
    public IEnumerable<LeaderboardDTO> GetLeaderboard()
    {
        var leaderboard = _context.Users
             .AsNoTracking()
             .Include(ut => ut.UserTournaments)
             .Include(ut => ut.MatchUsers)
                .ThenInclude(ut => ut.Match)
                .ThenInclude(ut => ut.TournamentRound)
             .Where(x => x.EmailConfirmed && x.UserTournaments.Any(ut => ut.WaitList ?? false))
             .GroupBy(ut => ut.Id)
             .Select(g => new LeaderboardDTO
             {
                 PlayerId = g.Key,
                 PlayerName = g.First().DisplayName == "" ? g.First().FirstName + " " + g.First().LastName : g.First().DisplayName,
                 PlayerProfilePictureUrl = g.FirstOrDefault().Picture != null ? g.First().Picture.FileUrl : "",
                 Tournaments = g.First().UserTournaments.Count,
                 TournamentList = g.First().UserTournaments
                                   .Select(ut => new TournamentMiniDTO(ut.TournamentId, ut.Tournament.Name, ut.Tournament.Description))
                                   .ToList(),
                 Points = g.First().MatchUsers
                       .Sum(ut => ut.Point ?? 0),
                 Win = g.First().MatchUsers
                       .Count(ut => ut.Win ?? false),
                 Loss = g.First().MatchUsers
                        .Count(ut => ut.Loss ?? false),
                 Draw = g.First().MatchUsers
                        .Count(ut => ut.Draw ?? false),
                 Form = g.First().MatchUsers.Take(5)
                        .Select(ut => new TournamentFormDTO(ut.MatchId, ut.Win ?? false, ut.Loss ?? false, ut.Draw ?? false))
                        .AsEnumerable()
             })
             .OrderByDescending(l => l.Points)
            .ThenBy(l => l.Tournaments)
            .ThenBy(l => l.PlayerName)
            .AsEnumerable()
            .Select((l, index) => new LeaderboardDTO
            {
                Rank = index + 1,
                PlayerId = l.PlayerId,
                PlayerName = l.PlayerName,
                PlayerProfilePictureUrl = l.PlayerProfilePictureUrl,
                Tournaments = l.Tournaments,
                Points = l.Points,
                TournamentList = l.TournamentList,
                Win = l.Win,
                Loss = l.Loss,
                Draw = l.Draw,
                Form = l.Form
            });
        return leaderboard;
    }

    /// <summary>
    /// Retrieves the leaderboard with player rankings based on points and tournaments filtered by tournamentId
    /// </summary>
    /// <returns>An enumerable collection of LeaderboardDTO objects representing the leaderboard.</returns>
    public IEnumerable<LeaderboardDTO> GetLeaderboardWithTournamentFilter(Guid tournamentId)
    {
        var leaderboard = _context.Users
            .AsNoTracking()
            .Include(ut => ut.UserTournaments)
            .Include(ut => ut.MatchUsers)
            .ThenInclude(ut => ut.Match)
            .ThenInclude(ut => ut.TournamentRound)
            .Where(x => x.EmailConfirmed && x.UserTournaments.Any(ut => ut.WaitList ?? false) && x.UserTournaments.Any(ut => ut.TournamentId == tournamentId))
            .GroupBy(ut => ut.Id)
            .Select(g => new LeaderboardDTO
            {
                PlayerId = g.Key,
                PlayerName = g.First().DisplayName == "" ? g.First().FirstName + " " + g.First().LastName : g.First().DisplayName,
                PlayerProfilePictureUrl = g.First().Picture != null ? g.First().Picture.FileUrl : "",
                Tournaments = g.First().UserTournaments.Count,
                TournamentList = g.First().UserTournaments.Select(ut => new TournamentMiniDTO(ut.TournamentId, ut.Tournament.Name, ut.Tournament.Description)).ToList(),
                Points = g.First().MatchUsers.Sum(ut => ut.Point ?? 0),
                Win = g.First().MatchUsers
                       .Where(x => x.Match.TournamentRound.TournamentId == tournamentId)
                       .Count(ut => ut.Win ?? false),
                Loss = g.First().MatchUsers
                        .Where(x => x.Match.TournamentRound.TournamentId == tournamentId)
                        .Count(ut => ut.Loss ?? false),
                Draw = g.First().MatchUsers
                        .Where(x => x.Match.TournamentRound.TournamentId == tournamentId)
                        .Count(ut => ut.Draw ?? false),
                Form = g.First().MatchUsers.Take(5)
                        .Select(ut => new TournamentFormDTO(ut.MatchId, ut.Win ?? false, ut.Loss ?? false, ut.Draw ?? false))
                        .AsEnumerable()
            })
            .OrderByDescending(l => l.Points)
           .ThenBy(l => l.Tournaments)
           .ThenBy(l => l.PlayerName)
           .AsEnumerable()
           .Select((l, index) => new LeaderboardDTO
           {
               Rank = index + 1,
               PlayerId = l.PlayerId,
               PlayerName = l.PlayerName,
               PlayerProfilePictureUrl = l.PlayerProfilePictureUrl,
               Tournaments = l.Tournaments,
               Points = l.Points,
               TournamentList = l.TournamentList,
               Win = l.Win,
               Loss = l.Loss,
               Draw = l.Draw,
               Form = l.Form
           });
        return leaderboard;
    }
}
