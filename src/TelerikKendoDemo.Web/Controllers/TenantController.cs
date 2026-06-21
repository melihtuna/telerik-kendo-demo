using Microsoft.AspNetCore.Mvc;
using TelerikKendoDemo.Application.Interfaces;
using TelerikKendoDemo.Web.Services;

namespace TelerikKendoDemo.Web.Controllers;

public class TenantController : Controller
{
    private readonly ITenantService _tenantService;

    public TenantController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTenants(CancellationToken cancellationToken)
    {
        var tenants = await _tenantService.GetAllAsync(cancellationToken);
        return Json(tenants);
    }

    [HttpPost]
    public IActionResult SetTenant([FromBody] TenantSelectionModel model)
    {
        HttpContext.Session.SetString(TenantSessionKeys.TenantId, model.TenantId.ToString());
        return Json(new { success = true });
    }

    public class TenantSelectionModel
    {
        public Guid TenantId { get; set; }
    }
}
