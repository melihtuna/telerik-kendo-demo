using Microsoft.EntityFrameworkCore;
using TelerikKendoDemo.Application.Common;
using TelerikKendoDemo.Application.DTOs;
using TelerikKendoDemo.Application.Interfaces;
using TelerikKendoDemo.Application.Mappings;
using TelerikKendoDemo.Domain.Entities;
using TelerikKendoDemo.Domain.Interfaces;

namespace TelerikKendoDemo.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;

    public DepartmentService(IUnitOfWork unitOfWork, ITenantContext tenantContext)
    {
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<IReadOnlyList<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var departments = await _unitOfWork.Departments.Query()
            .Where(d => d.TenantId == _tenantContext.TenantId)
            .OrderBy(d => d.Name)
            .ToListAsync(cancellationToken);

        var parentIds = departments
            .Where(d => d.ParentDepartmentId.HasValue)
            .Select(d => d.ParentDepartmentId!.Value)
            .ToHashSet();

        return departments
            .Select(d => d.ToDto(parentIds.Contains(d.Id)))
            .ToList();
    }

    public async Task<IReadOnlyList<DepartmentTreeItemDto>> GetTreeAsync(Guid? parentDepartmentId = null, CancellationToken cancellationToken = default)
    {
        var departments = await _unitOfWork.Departments.Query()
            .Where(d => d.TenantId == _tenantContext.TenantId)
            .OrderBy(d => d.Name)
            .ToListAsync(cancellationToken);

        if (parentDepartmentId.HasValue)
        {
            return departments
                .Where(d => d.ParentDepartmentId == parentDepartmentId)
                .Select(d => new DepartmentTreeItemDto
                {
                    Id = d.Id,
                    Text = d.Name,
                    HasChildren = departments.Any(child => child.ParentDepartmentId == d.Id)
                })
                .ToList();
        }

        var nodes = departments.ToDictionary(
            d => d.Id,
            d => new DepartmentTreeItemDto
            {
                Id = d.Id,
                Text = d.Name
            });

        var roots = new List<DepartmentTreeItemDto>();

        foreach (var department in departments)
        {
            var node = nodes[department.Id];

            if (department.ParentDepartmentId is null)
            {
                node.Expanded = true;
                roots.Add(node);
                continue;
            }

            if (nodes.TryGetValue(department.ParentDepartmentId.Value, out var parent))
            {
                parent.Items.Add(node);
            }
        }

        foreach (var node in nodes.Values)
        {
            node.HasChildren = node.Items.Count > 0;
        }

        return roots;
    }

    public async Task<DepartmentDto> CreateAsync(string name, Guid? parentDepartmentId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessException("Departman adı zorunludur.");
        }

        if (parentDepartmentId.HasValue)
        {
            var parentExists = await _unitOfWork.Departments.Query()
                .AnyAsync(d => d.Id == parentDepartmentId && d.TenantId == _tenantContext.TenantId, cancellationToken);

            if (!parentExists)
            {
                throw new NotFoundException("Üst departman bulunamadı.");
            }
        }

        var now = DateTime.UtcNow;
        var department = new Department
        {
            Id = Guid.NewGuid(),
            TenantId = _tenantContext.TenantId,
            Name = name.Trim(),
            ParentDepartmentId = parentDepartmentId,
            CreatedDate = now,
            UpdatedDate = now
        };

        await _unitOfWork.Departments.AddAsync(department, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return department.ToDto();
    }

    public async Task<DepartmentDto> UpdateAsync(Guid id, string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessException("Departman adı zorunludur.");
        }

        var department = await _unitOfWork.Departments.Query()
            .FirstOrDefaultAsync(d => d.Id == id && d.TenantId == _tenantContext.TenantId, cancellationToken);

        if (department is null)
        {
            throw new NotFoundException("Departman bulunamadı.");
        }

        department.Name = name.Trim();
        department.UpdatedDate = DateTime.UtcNow;

        _unitOfWork.Departments.Update(department);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var hasChildren = await _unitOfWork.Departments.Query()
            .AnyAsync(d => d.ParentDepartmentId == id, cancellationToken);

        return department.ToDto(hasChildren);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var department = await _unitOfWork.Departments.Query()
            .FirstOrDefaultAsync(d => d.Id == id && d.TenantId == _tenantContext.TenantId, cancellationToken);

        if (department is null)
        {
            throw new NotFoundException("Departman bulunamadı.");
        }

        var hasChildren = await _unitOfWork.Departments.Query()
            .AnyAsync(d => d.ParentDepartmentId == id, cancellationToken);

        if (hasChildren)
        {
            throw new BusinessException("Alt departmanları olan bir departman silinemez.");
        }

        var hasEmployees = await _unitOfWork.Employees.Query()
            .AnyAsync(e => e.DepartmentId == id, cancellationToken);

        if (hasEmployees)
        {
            throw new BusinessException("Çalışanı bulunan bir departman silinemez.");
        }

        _unitOfWork.Departments.Remove(department);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
