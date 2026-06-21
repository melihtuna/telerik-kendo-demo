using Microsoft.EntityFrameworkCore;
using TelerikKendoDemo.Application.Common;
using TelerikKendoDemo.Application.DTOs;
using TelerikKendoDemo.Application.Interfaces;
using TelerikKendoDemo.Application.Mappings;
using TelerikKendoDemo.Domain.Interfaces;

namespace TelerikKendoDemo.Application.Services;

public class SkillService : ISkillService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;

    public SkillService(IUnitOfWork unitOfWork, ITenantContext tenantContext)
    {
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<IReadOnlyList<SkillDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var skills = await _unitOfWork.Skills.Query()
            .Where(s => s.TenantId == _tenantContext.TenantId)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);

        return skills.Select(s => s.ToDto()).ToList();
    }
}
