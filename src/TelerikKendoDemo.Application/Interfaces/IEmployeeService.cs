using TelerikKendoDemo.Application.DTOs;

namespace TelerikKendoDemo.Application.Interfaces;

public interface IEmployeeService
{
    Task<IReadOnlyList<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<EmployeeDto> CreateAsync(EmployeeCreateUpdateDto dto, CancellationToken cancellationToken = default);
    Task<EmployeeDto> UpdateAsync(EmployeeCreateUpdateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
