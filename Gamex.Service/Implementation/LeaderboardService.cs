namespace Gamex.Service.Implementation;

public class LeaderboardService(GamexDbContext context) : ILeaderboardService
{
    private readonly GamexDbContext _context = context;

    public IEnumerable<LeaderboardDTO> GetLeaderboard()
    {
        var leaderboard = _context.UserTournaments
            .AsNoTracking()
            .Include(ut => ut.User)
            .GroupBy(ut => ut.UserId)
            .Select(g => new LeaderboardDTO
            {
                PlayerId = g.Key,
                PlayerName = g.First().User.DisplayName,
                PlayerProfilePictureUrl = g.First().User.Picture != null ? g.First().User.Picture.FileUrl : "",
                Tournaments = g.Count(),
                Points = g.Sum(ut => ut.Point ?? 0),
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
