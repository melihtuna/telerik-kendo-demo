using Microsoft.AspNetCore.Mvc;
using TelerikKendoDemo.Application.DTOs;
using TelerikKendoDemo.Application.Interfaces;

namespace TelerikKendoDemo.Web.Controllers;

public class LeaveRequestsController : Controller
{
    private readonly ILeaveRequestService _leaveRequestService;
    private readonly IEmployeeService _employeeService;

    public LeaveRequestsController(
        ILeaveRequestService leaveRequestService,
        IEmployeeService employeeService)
    {
        _leaveRequestService = leaveRequestService;
        _employeeService = employeeService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Read(CancellationToken cancellationToken)
    {
        var leaveRequests = await _leaveRequestService.GetAllAsync(cancellationToken);
        var schedulerData = leaveRequests.Select(l => new
        {
            l.Id,
            title = l.Title,
            start = l.StartDate,
            end = l.EndDate.AddDays(1),
            employeeId = l.EmployeeId,
            employeeName = l.EmployeeName,
            reason = l.Reason,
            status = l.Status,
            statusText = l.StatusText
        });

        return Json(schedulerData);
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees(CancellationToken cancellationToken)
    {
        var employees = await _employeeService.GetAllAsync(cancellationToken);
        return Json(employees.Select(e => new { e.Id, Name = e.FullName }));
    }

    [HttpGet]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var leaveRequest = await _leaveRequestService.GetByIdAsync(id, cancellationToken);
        if (leaveRequest is null)
        {
            return NotFound(new { success = false, message = "İzin kaydı bulunamadı." });
        }

        return Json(leaveRequest);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LeaveRequestCreateUpdateDto model, CancellationToken cancellationToken)
    {
        var leaveRequest = await _leaveRequestService.CreateAsync(model, cancellationToken);
        return Json(new { success = true, message = "İzin talebi başarıyla oluşturuldu.", data = leaveRequest });
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] LeaveRequestCreateUpdateDto model, CancellationToken cancellationToken)
    {
        var leaveRequest = await _leaveRequestService.UpdateAsync(model, cancellationToken);
        return Json(new { success = true, message = "İzin talebi başarıyla güncellendi.", data = leaveRequest });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _leaveRequestService.DeleteAsync(id, cancellationToken);
        return Json(new { success = true, message = "İzin talebi başarıyla silindi." });
    }
}
