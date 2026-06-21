using TelerikKendoDemo.Application.DTOs;

namespace TelerikKendoDemo.Application.Interfaces;

public interface IDepartmentService
{
    Task<IReadOnlyList<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DepartmentTreeItemDto>> GetTreeAsync(Guid? parentDepartmentId = null, CancellationToken cancellationToken = default);
    Task<DepartmentDto> CreateAsync(string name, Guid? parentDepartmentId, CancellationToken cancellationToken = default);
    Task<DepartmentDto> UpdateAsync(Guid id, string name, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
