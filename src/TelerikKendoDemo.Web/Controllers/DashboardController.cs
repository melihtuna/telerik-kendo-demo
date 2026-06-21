using Microsoft.AspNetCore.Mvc;
using TelerikKendoDemo.Application.Interfaces;

namespace TelerikKendoDemo.Web.Controllers;

public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await _dashboardService.GetDashboardAsync(cancellationToken);
        return View(model);
    }
}
