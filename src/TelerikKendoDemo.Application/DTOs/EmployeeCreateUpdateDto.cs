namespace TelerikKendoDemo.Application.DTOs;

public class EmployeeCreateUpdateDto
{
    public Guid? Id { get; set; }
    public Guid DepartmentId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime HireDate { get; set; }
    public string? PhotoUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public IList<Guid> SkillIds { get; set; } = new List<Guid>();
}
