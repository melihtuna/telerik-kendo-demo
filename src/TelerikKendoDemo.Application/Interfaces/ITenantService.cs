using TelerikKendoDemo.Application.DTOs;

namespace TelerikKendoDemo.Application.Interfaces;

public interface ITenantService
{
    Task<IReadOnlyList<TenantDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TenantDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
