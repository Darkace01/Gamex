namespace Gamex.Service.Contract
{
    public interface ILeaderboardService
    {
        /// <summary>
        /// Retrieves the leaderboard.
        /// </summary>
        /// <returns>The collection of leaderboard DTOs.</returns>
        IEnumerable<LeaderboardDTO> GetLeaderboard();
    }
}