using TelerikKendoDemo.Application.DTOs;

namespace TelerikKendoDemo.Application.Interfaces;

public interface ISkillService
{
    Task<IReadOnlyList<SkillDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
