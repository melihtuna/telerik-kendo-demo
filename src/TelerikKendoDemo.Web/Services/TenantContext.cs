using TelerikKendoDemo.Application.Common;

namespace TelerikKendoDemo.Web.Services;

public class TenantContext : ITenantContext
{
    private Guid _tenantId;

    public Guid TenantId => _tenantId;

    public void SetTenantId(Guid tenantId)
    {
        _tenantId = tenantId;
    }
}
