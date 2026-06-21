using TelerikKendoDemo.Application.DTOs;
using TelerikKendoDemo.Domain.Entities;
using TelerikKendoDemo.Domain.Enums;

namespace TelerikKendoDemo.Application.Mappings;

public static class EntityMapper
{
    public static TenantDto ToDto(this Tenant entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name
    };

    public static SkillDto ToDto(this Skill entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name
    };

    public static DepartmentDto ToDto(this Department entity, bool hasChildren = false) => new()
    {
        Id = entity.Id,
        TenantId = entity.TenantId,
        Name = entity.Name,
        ParentDepartmentId = entity.ParentDepartmentId,
        HasChildren = hasChildren
    };

    public static EmployeeDto ToDto(this Employee entity) => new()
    {
        Id = entity.Id,
        TenantId = entity.TenantId,
        DepartmentId = entity.DepartmentId,
        DepartmentName = entity.Department?.Name ?? string.Empty,
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        FullName = entity.FullName,
        Email = entity.Email,
        Phone = entity.Phone,
        BirthDate = entity.BirthDate,
        HireDate = entity.HireDate,
        PhotoUrl = entity.PhotoUrl,
        IsActive = entity.IsActive,
        SkillIds = entity.EmployeeSkills.Select(es => es.SkillId).ToList(),
        SkillNames = entity.EmployeeSkills.Select(es => es.Skill?.Name ?? string.Empty).Where(n => !string.IsNullOrEmpty(n)).ToList()
    };

    public static LeaveRequestDto ToDto(this LeaveRequest entity) => new()
    {
        Id = entity.Id,
        TenantId = entity.TenantId,
        EmployeeId = entity.EmployeeId,
        EmployeeName = entity.Employee?.FullName ?? string.Empty,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        Reason = entity.Reason,
        Status = (int)entity.Status,
        StatusText = GetStatusText(entity.Status),
        Title = $"{entity.Employee?.FullName ?? "Çalışan"} - {GetStatusText(entity.Status)}"
    };

    public static string GetStatusText(LeaveRequestStatus status) => status switch
    {
        LeaveRequestStatus.Bekliyor => "Bekliyor",
        LeaveRequestStatus.Onaylandi => "Onaylandı",
        LeaveRequestStatus.Reddedildi => "Reddedildi",
        _ => status.ToString()
    };
}
