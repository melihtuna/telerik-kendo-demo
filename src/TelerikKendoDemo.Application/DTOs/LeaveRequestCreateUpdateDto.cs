namespace TelerikKendoDemo.Application.DTOs;

public class LeaveRequestCreateUpdateDto
{
    public Guid? Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public int Status { get; set; }
}
