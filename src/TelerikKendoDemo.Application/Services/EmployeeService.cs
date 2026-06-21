using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TelerikKendoDemo.Application.Common;
using TelerikKendoDemo.Application.DTOs;
using TelerikKendoDemo.Application.Interfaces;
using TelerikKendoDemo.Application.Mappings;
using TelerikKendoDemo.Domain.Entities;
using TelerikKendoDemo.Domain.Interfaces;

namespace TelerikKendoDemo.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;
    private readonly IValidator<EmployeeCreateUpdateDto> _validator;

    public EmployeeService(
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext,
        IValidator<EmployeeCreateUpdateDto> validator)
    {
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
        _validator = validator;
    }

    public async Task<IReadOnlyList<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var employees = await _unitOfWork.Employees.Query()
            .Where(e => e.TenantId == _tenantContext.TenantId)
            .Include(e => e.Department)
            .Include(e => e.EmployeeSkills)
                .ThenInclude(es => es.Skill)
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync(cancellationToken);

        return employees.Select(e => e.ToDto()).ToList();
    }

    public async Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var employee = await _unitOfWork.Employees.Query()
            .Where(e => e.TenantId == _tenantContext.TenantId && e.Id == id)
            .Include(e => e.Department)
            .Include(e => e.EmployeeSkills)
                .ThenInclude(es => es.Skill)
            .FirstOrDefaultAsync(cancellationToken);

        return employee?.ToDto();
    }

    public async Task<EmployeeDto> CreateAsync(EmployeeCreateUpdateDto dto, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(dto, cancellationToken);

        var departmentExists = await _unitOfWork.Departments.Query()
            .AnyAsync(d => d.Id == dto.DepartmentId && d.TenantId == _tenantContext.TenantId, cancellationToken);

        if (!departmentExists)
        {
            throw new NotFoundException("Seçilen departman bulunamadı.");
        }

        var now = DateTime.UtcNow;
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            TenantId = _tenantContext.TenantId,
            DepartmentId = dto.DepartmentId,
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            Email = dto.Email.Trim().ToLowerInvariant(),
            Phone = dto.Phone?.Trim(),
            BirthDate = DateTimeHelper.ToUtcDate(dto.BirthDate),
            HireDate = DateTimeHelper.ToUtcDate(dto.HireDate),
            PhotoUrl = dto.PhotoUrl,
            IsActive = dto.IsActive,
            CreatedDate = now,
            UpdatedDate = now
        };

        await _unitOfWork.Employees.AddAsync(employee, cancellationToken);
        await UpdateEmployeeSkillsAsync(employee.Id, dto.SkillIds, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return (await GetByIdAsync(employee.Id, cancellationToken))!;
    }

    public async Task<EmployeeDto> UpdateAsync(EmployeeCreateUpdateDto dto, CancellationToken cancellationToken = default)
    {
        if (!dto.Id.HasValue)
        {
            throw new BusinessException("Güncelleme için çalışan kimliği gereklidir.");
        }

        await _validator.ValidateAndThrowAsync(dto, cancellationToken);

        var employee = await _unitOfWork.Employees.Query()
            .Where(e => e.TenantId == _tenantContext.TenantId && e.Id == dto.Id.Value)
            .Include(e => e.EmployeeSkills)
            .FirstOrDefaultAsync(cancellationToken);

        if (employee is null)
        {
            throw new NotFoundException("Çalışan bulunamadı.");
        }

        employee.DepartmentId = dto.DepartmentId;
        employee.FirstName = dto.FirstName.Trim();
        employee.LastName = dto.LastName.Trim();
        employee.Email = dto.Email.Trim().ToLowerInvariant();
        employee.Phone = dto.Phone?.Trim();
        employee.BirthDate = DateTimeHelper.ToUtcDate(dto.BirthDate);
        employee.HireDate = DateTimeHelper.ToUtcDate(dto.HireDate);
        employee.PhotoUrl = dto.PhotoUrl;
        employee.IsActive = dto.IsActive;
        employee.UpdatedDate = DateTime.UtcNow;

        _unitOfWork.Employees.Update(employee);
        await UpdateEmployeeSkillsAsync(employee.Id, dto.SkillIds, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return (await GetByIdAsync(employee.Id, cancellationToken))!;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var employee = await _unitOfWork.Employees.Query()
            .Where(e => e.TenantId == _tenantContext.TenantId && e.Id == id)
            .Include(e => e.EmployeeSkills)
            .FirstOrDefaultAsync(cancellationToken);

        if (employee is null)
        {
            throw new NotFoundException("Çalışan bulunamadı.");
        }

        foreach (var skill in employee.EmployeeSkills.ToList())
        {
            employee.EmployeeSkills.Remove(skill);
        }

        _unitOfWork.Employees.Remove(employee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task UpdateEmployeeSkillsAsync(Guid employeeId, IList<Guid> skillIds, CancellationToken cancellationToken)
    {
        var employee = await _unitOfWork.Employees.Query()
            .Where(e => e.Id == employeeId)
            .Include(e => e.EmployeeSkills)
            .FirstAsync(cancellationToken);

        employee.EmployeeSkills.Clear();

        var validSkillIds = await _unitOfWork.Skills.Query()
            .Where(s => s.TenantId == _tenantContext.TenantId && skillIds.Contains(s.Id))
            .Select(s => s.Id)
            .ToListAsync(cancellationToken);

        foreach (var skillId in validSkillIds)
        {
            employee.EmployeeSkills.Add(new EmployeeSkill
            {
                EmployeeId = employeeId,
                SkillId = skillId
            });
        }
    }
}
