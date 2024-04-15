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
             .GroupBy(ut => ut.Id)
             .Select(g => new LeaderboardDTO
             {
                 PlayerId = g.Key,
                 PlayerName = g.First().DisplayName == "" ? g.First().FirstName + " " + g.First().LastName : g.First().DisplayName,
                 PlayerProfilePictureUrl = g.First().Picture != null ? g.First().Picture.FileUrl : "",
                 Tournaments = g.First().UserTournaments.Count,
                 Points = g.First().UserTournaments.Sum(ut => ut.Point ?? 0)
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
                Points = l.Points
            });
        return leaderboard;
    }
}
