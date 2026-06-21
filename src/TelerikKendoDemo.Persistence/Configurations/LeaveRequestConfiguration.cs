using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelerikKendoDemo.Domain.Entities;

namespace TelerikKendoDemo.Persistence.Configurations;

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.ToTable("LeaveRequests");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Reason).IsRequired().HasMaxLength(500);

        builder.HasOne(l => l.Tenant)
            .WithMany(t => t.LeaveRequests)
            .HasForeignKey(l => l.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Employee)
            .WithMany(e => e.LeaveRequests)
            .HasForeignKey(l => l.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
