using TelerikKendoDemo.Application.DTOs;

namespace TelerikKendoDemo.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default);
}
