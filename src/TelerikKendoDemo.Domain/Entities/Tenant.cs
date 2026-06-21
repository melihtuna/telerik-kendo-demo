namespace TelerikKendoDemo.Domain.Entities;

public class Tenant : Common.BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Department> Departments { get; set; } = new List<Department>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}
