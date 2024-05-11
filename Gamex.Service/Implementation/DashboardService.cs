namespace Gamex.Service.Implementation;
public class DashboardService(GamexDbContext context) : IDashboardService
{
    public async Task<DashboardDTO> GetDashboardStats(CancellationToken cancellationToken = default)
    {
        var registeredUserCount = await context.Users.CountAsync(cancellationToken);
        var tournamentCount = await context.Tournaments.CountAsync(cancellationToken);
        var usersInWaitList = await context.UserTournaments.CountAsync(ut => ut.WaitList ?? false, cancellationToken);
        var postsCount = await context.Posts.CountAsync(cancellationToken);

        return new DashboardDTO
        {
            RegisteredUsers = registeredUserCount,
            Tournaments = tournamentCount,
            UsersInWaitlist = usersInWaitList,
            Posts = postsCount
        };
    }
}
