using TelerikKendoDemo.Domain.Entities;

namespace TelerikKendoDemo.Domain.Interfaces;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRepository<Tenant> Tenants { get; }
    IRepository<Department> Departments { get; }
    IRepository<Employee> Employees { get; }
    IRepository<Skill> Skills { get; }
    IRepository<LeaveRequest> LeaveRequests { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
