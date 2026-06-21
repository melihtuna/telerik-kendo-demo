using TelerikKendoDemo.Application.DTOs;

namespace TelerikKendoDemo.Application.Interfaces;

public interface ILeaveRequestService
{
    Task<IReadOnlyList<LeaveRequestDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LeaveRequestDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<LeaveRequestDto> CreateAsync(LeaveRequestCreateUpdateDto dto, CancellationToken cancellationToken = default);
    Task<LeaveRequestDto> UpdateAsync(LeaveRequestCreateUpdateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
