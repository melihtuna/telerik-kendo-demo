namespace TelerikKendoDemo.Application.DTOs;

public class LeaveRequestDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public int Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}
