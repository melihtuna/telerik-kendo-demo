using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TelerikKendoDemo.Application.Common;
using TelerikKendoDemo.Application.Interfaces;
using TelerikKendoDemo.Web.Services;

namespace TelerikKendoDemo.Web.Filters;

public class TenantActionFilter : IAsyncActionFilter
{
    private readonly ITenantContext _tenantContext;
    private readonly ITenantService _tenantService;

    public TenantActionFilter(ITenantContext tenantContext, ITenantService tenantService)
    {
        _tenantContext = tenantContext;
        _tenantService = tenantService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        var session = httpContext.Session;

        if (session.GetString(TenantSessionKeys.TenantId) is null)
        {
            var tenants = await _tenantService.GetAllAsync();
            var defaultTenant = tenants.FirstOrDefault(t => t.Name == TenantSessionKeys.DefaultTenantName)
                                ?? tenants.FirstOrDefault();

            if (defaultTenant is not null)
            {
                session.SetString(TenantSessionKeys.TenantId, defaultTenant.Id.ToString());
            }
        }

        if (Guid.TryParse(session.GetString(TenantSessionKeys.TenantId), out var tenantId))
        {
            _tenantContext.SetTenantId(tenantId);
        }

        await next();
    }
}
