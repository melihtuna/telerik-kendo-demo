namespace TelerikKendoDemo.Application.DTOs;

public class DashboardDto
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public int DepartmentCount { get; set; }
    public int PendingLeaveRequests { get; set; }
    public double ActiveEmployeePercentage { get; set; }
    public double LeaveApprovalProgress { get; set; }
    public IList<ChartDataPointDto> DepartmentDistribution { get; set; } = new List<ChartDataPointDto>();
}

public class ChartDataPointDto
{
    public string Category { get; set; } = string.Empty;
    public int Value { get; set; }
}
