namespace TelerikKendoDemo.Domain.Entities;

public class Skill : Common.BaseEntity, Common.ITenantEntity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;

    public Tenant Tenant { get; set; } = null!;
    public ICollection<EmployeeSkill> EmployeeSkills { get; set; } = new List<EmployeeSkill>();
}
