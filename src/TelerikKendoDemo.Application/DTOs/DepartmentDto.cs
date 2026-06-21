namespace TelerikKendoDemo.Application.DTOs;

public class DepartmentDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? ParentDepartmentId { get; set; }
    public bool HasChildren { get; set; }
}
