using Microsoft.EntityFrameworkCore;
using TelerikKendoDemo.Application.Common;
using TelerikKendoDemo.Application.DTOs;
using TelerikKendoDemo.Application.Interfaces;
using TelerikKendoDemo.Application.Mappings;
using TelerikKendoDemo.Domain.Interfaces;

namespace TelerikKendoDemo.Application.Services;

public class TenantService : ITenantService
{
    private readonly IUnitOfWork _unitOfWork;

    public TenantService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TenantDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var tenants = await _unitOfWork.Tenants.Query()
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);

        return tenants.Select(t => t.ToDto()).ToList();
    }

    public async Task<TenantDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tenant = await _unitOfWork.Tenants.GetByIdAsync(id, cancellationToken);
        return tenant?.ToDto();
    }
}
