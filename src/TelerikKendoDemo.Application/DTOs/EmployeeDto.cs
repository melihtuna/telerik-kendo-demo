namespace TelerikKendoDemo.Application.DTOs;

public class EmployeeDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime HireDate { get; set; }
    public string? PhotoUrl { get; set; }
    public bool IsActive { get; set; }
    public IList<Guid> SkillIds { get; set; } = new List<Guid>();
    public IList<string> SkillNames { get; set; } = new List<string>();
}
