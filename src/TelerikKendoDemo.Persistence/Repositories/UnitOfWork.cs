using TelerikKendoDemo.Domain.Entities;
using TelerikKendoDemo.Domain.Interfaces;
using TelerikKendoDemo.Persistence.Context;

namespace TelerikKendoDemo.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Tenants = new Repository<Tenant>(context);
        Departments = new Repository<Department>(context);
        Employees = new Repository<Employee>(context);
        Skills = new Repository<Skill>(context);
        LeaveRequests = new Repository<LeaveRequest>(context);
    }

    public IRepository<Tenant> Tenants { get; }
    public IRepository<Department> Departments { get; }
    public IRepository<Employee> Employees { get; }
    public IRepository<Skill> Skills { get; }
    public IRepository<LeaveRequest> LeaveRequests { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
