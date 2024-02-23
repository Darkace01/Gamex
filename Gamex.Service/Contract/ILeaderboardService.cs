namespace Gamex.Service.Contract
{
    public interface ILeaderboardService
    {
        IEnumerable<LeaderboardDTO> GetLeaderboard();
    }
}