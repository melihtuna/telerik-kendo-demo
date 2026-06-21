using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelerikKendoDemo.Domain.Entities;

namespace TelerikKendoDemo.Persistence.Configurations;

public class EmployeeSkillConfiguration : IEntityTypeConfiguration<EmployeeSkill>
{
    public void Configure(EntityTypeBuilder<EmployeeSkill> builder)
    {
        builder.ToTable("EmployeeSkills");
        builder.HasKey(es => new { es.EmployeeId, es.SkillId });

        builder.HasOne(es => es.Employee)
            .WithMany(e => e.EmployeeSkills)
            .HasForeignKey(es => es.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(es => es.Skill)
            .WithMany(s => s.EmployeeSkills)
            .HasForeignKey(es => es.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
