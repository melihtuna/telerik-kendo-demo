namespace TelerikKendoDemo.Domain.Entities;

public class Employee : Common.BaseEntity, Common.ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid DepartmentId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime HireDate { get; set; }
    public string? PhotoUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public Tenant Tenant { get; set; } = null!;
    public Department Department { get; set; } = null!;
    public ICollection<EmployeeSkill> EmployeeSkills { get; set; } = new List<EmployeeSkill>();
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public string FullName => $"{FirstName} {LastName}";
}
