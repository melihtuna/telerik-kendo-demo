namespace TelerikKendoDemo.Domain.Entities;

public class EmployeeSkill
{
    public Guid EmployeeId { get; set; }
    public Guid SkillId { get; set; }

    public Employee Employee { get; set; } = null!;
    public Skill Skill { get; set; } = null!;
}
