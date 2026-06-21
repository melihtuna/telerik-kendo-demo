namespace TelerikKendoDemo.Domain.Entities;

public class Department : Common.BaseEntity, Common.ITenantEntity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? ParentDepartmentId { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public Department? ParentDepartment { get; set; }
    public ICollection<Department> SubDepartments { get; set; } = new List<Department>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
