using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TelerikKendoDemo.Application.Common;
using TelerikKendoDemo.Application.DTOs;
using TelerikKendoDemo.Application.Interfaces;
using TelerikKendoDemo.Application.Mappings;
using TelerikKendoDemo.Domain.Entities;
using TelerikKendoDemo.Domain.Enums;
using TelerikKendoDemo.Domain.Interfaces;

namespace TelerikKendoDemo.Application.Services;

public class LeaveRequestService : ILeaveRequestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;
    private readonly IValidator<LeaveRequestCreateUpdateDto> _validator;

    public LeaveRequestService(
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext,
        IValidator<LeaveRequestCreateUpdateDto> validator)
    {
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
        _validator = validator;
    }

    public async Task<IReadOnlyList<LeaveRequestDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var leaveRequests = await _unitOfWork.LeaveRequests.Query()
            .Where(l => l.TenantId == _tenantContext.TenantId)
            .Include(l => l.Employee)
            .OrderByDescending(l => l.StartDate)
            .ToListAsync(cancellationToken);

        return leaveRequests.Select(l => l.ToDto()).ToList();
    }

    public async Task<LeaveRequestDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var leaveRequest = await _unitOfWork.LeaveRequests.Query()
            .Where(l => l.TenantId == _tenantContext.TenantId && l.Id == id)
            .Include(l => l.Employee)
            .FirstOrDefaultAsync(cancellationToken);

        return leaveRequest?.ToDto();
    }

    public async Task<LeaveRequestDto> CreateAsync(LeaveRequestCreateUpdateDto dto, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(dto, cancellationToken);

        var employeeExists = await _unitOfWork.Employees.Query()
            .AnyAsync(e => e.Id == dto.EmployeeId && e.TenantId == _tenantContext.TenantId, cancellationToken);

        if (!employeeExists)
        {
            throw new NotFoundException("Seçilen çalışan bulunamadı.");
        }

        var now = DateTime.UtcNow;
        var leaveRequest = new LeaveRequest
        {
            Id = Guid.NewGuid(),
            TenantId = _tenantContext.TenantId,
            EmployeeId = dto.EmployeeId,
            StartDate = DateTimeHelper.ToUtcDate(dto.StartDate),
            EndDate = DateTimeHelper.ToUtcDate(dto.EndDate),
            Reason = dto.Reason.Trim(),
            Status = (LeaveRequestStatus)dto.Status,
            CreatedDate = now,
            UpdatedDate = now
        };

        await _unitOfWork.LeaveRequests.AddAsync(leaveRequest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return (await GetByIdAsync(leaveRequest.Id, cancellationToken))!;
    }

    public async Task<LeaveRequestDto> UpdateAsync(LeaveRequestCreateUpdateDto dto, CancellationToken cancellationToken = default)
    {
        if (!dto.Id.HasValue)
        {
            throw new BusinessException("Güncelleme için izin kaydı kimliği gereklidir.");
        }

        await _validator.ValidateAndThrowAsync(dto, cancellationToken);

        var leaveRequest = await _unitOfWork.LeaveRequests.Query()
            .FirstOrDefaultAsync(l => l.TenantId == _tenantContext.TenantId && l.Id == dto.Id.Value, cancellationToken);

        if (leaveRequest is null)
        {
            throw new NotFoundException("İzin kaydı bulunamadı.");
        }

        leaveRequest.EmployeeId = dto.EmployeeId;
        leaveRequest.StartDate = DateTimeHelper.ToUtcDate(dto.StartDate);
        leaveRequest.EndDate = DateTimeHelper.ToUtcDate(dto.EndDate);
        leaveRequest.Reason = dto.Reason.Trim();
        leaveRequest.Status = (LeaveRequestStatus)dto.Status;
        leaveRequest.UpdatedDate = DateTime.UtcNow;

        _unitOfWork.LeaveRequests.Update(leaveRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return (await GetByIdAsync(leaveRequest.Id, cancellationToken))!;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var leaveRequest = await _unitOfWork.LeaveRequests.Query()
            .FirstOrDefaultAsync(l => l.TenantId == _tenantContext.TenantId && l.Id == id, cancellationToken);

        if (leaveRequest is null)
        {
            throw new NotFoundException("İzin kaydı bulunamadı.");
        }

        _unitOfWork.LeaveRequests.Remove(leaveRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
