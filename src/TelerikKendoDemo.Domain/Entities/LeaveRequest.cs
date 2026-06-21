namespace TelerikKendoDemo.Domain.Entities;

public class LeaveRequest : Common.BaseEntity, Common.ITenantEntity
{
    public Guid TenantId { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public Enums.LeaveRequestStatus Status { get; set; } = Enums.LeaveRequestStatus.Bekliyor;

    public Tenant Tenant { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
}
