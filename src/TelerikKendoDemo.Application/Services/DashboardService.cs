using Microsoft.EntityFrameworkCore;
using TelerikKendoDemo.Application.Common;
using TelerikKendoDemo.Application.DTOs;
using TelerikKendoDemo.Application.Interfaces;
using TelerikKendoDemo.Domain.Enums;
using TelerikKendoDemo.Domain.Interfaces;

namespace TelerikKendoDemo.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;

    public DashboardService(IUnitOfWork unitOfWork, ITenantContext tenantContext)
    {
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<DashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = _tenantContext.TenantId;

        var employees = await _unitOfWork.Employees.Query()
            .Where(e => e.TenantId == tenantId)
            .Include(e => e.Department)
            .ToListAsync(cancellationToken);

        var departmentCount = await _unitOfWork.Departments.Query()
            .CountAsync(d => d.TenantId == tenantId, cancellationToken);

        var pendingLeaves = await _unitOfWork.LeaveRequests.Query()
            .CountAsync(l => l.TenantId == tenantId && l.Status == LeaveRequestStatus.Bekliyor, cancellationToken);

        var totalLeaves = await _unitOfWork.LeaveRequests.Query()
            .CountAsync(l => l.TenantId == tenantId, cancellationToken);

        var approvedLeaves = await _unitOfWork.LeaveRequests.Query()
            .CountAsync(l => l.TenantId == tenantId && l.Status == LeaveRequestStatus.Onaylandi, cancellationToken);

        var totalEmployees = employees.Count;
        var activeEmployees = employees.Count(e => e.IsActive);

        var departmentDistribution = employees
            .GroupBy(e => e.Department?.Name ?? "Belirsiz")
            .Select(g => new ChartDataPointDto
            {
                Category = g.Key,
                Value = g.Count()
            })
            .OrderByDescending(x => x.Value)
            .ToList();

        return new DashboardDto
        {
            TotalEmployees = totalEmployees,
            ActiveEmployees = activeEmployees,
            DepartmentCount = departmentCount,
            PendingLeaveRequests = pendingLeaves,
            ActiveEmployeePercentage = totalEmployees == 0 ? 0 : Math.Round((double)activeEmployees / totalEmployees * 100, 1),
            LeaveApprovalProgress = totalLeaves == 0 ? 0 : Math.Round((double)approvedLeaves / totalLeaves * 100, 1),
            DepartmentDistribution = departmentDistribution
        };
    }
}
