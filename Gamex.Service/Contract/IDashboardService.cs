namespace Gamex.Service.Contract;

public interface IDashboardService
{
    Task<DashboardDTO> GetDashboardStats(CancellationToken cancellationToken = default);
}