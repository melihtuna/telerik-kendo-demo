namespace TelerikKendoDemo.Application.Common;

public interface ITenantContext
{
    Guid TenantId { get; }
    void SetTenantId(Guid tenantId);
}
