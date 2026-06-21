using Microsoft.EntityFrameworkCore;
using TelerikKendoDemo.Application.Common;
using TelerikKendoDemo.Domain.Entities;
using TelerikKendoDemo.Domain.Enums;
using TelerikKendoDemo.Persistence.Context;

namespace TelerikKendoDemo.Persistence.Seed;

public static class DatabaseSeeder
{
    private static readonly string[] TenantNames = ["ABC Holding", "XYZ Teknoloji", "Demo Şirketi"];

    private static readonly string[] DepartmentNames =
    [
        "Genel Müdürlük",
        "İnsan Kaynakları",
        "Bilgi Teknolojileri",
        "Finans",
        "Satış ve Pazarlama"
    ];

    private static readonly string[] SkillNames =
    [
        "C#", "ASP.NET Core", "PostgreSQL", "Entity Framework", "JavaScript",
        "React", "Proje Yönetimi", "İletişim", "Liderlik", "Analitik Düşünme"
    ];

    private static readonly string[] FirstNames =
    [
        "Ahmet", "Mehmet", "Ayşe", "Fatma", "Ali", "Zeynep", "Mustafa", "Elif",
        "Emre", "Deniz", "Burak", "Selin", "Can", "Merve", "Oğuz", "Ece",
        "Kerem", "Gizem", "Barış", "Pınar", "Serkan", "Derya", "Tolga", "Aslı",
        "Murat", "Cem", "Hande", "Volkan", "Seda", "Onur"
    ];

    private static readonly string[] LastNames =
    [
        "Yılmaz", "Kaya", "Demir", "Şahin", "Çelik", "Yıldız", "Aydın", "Öztürk",
        "Arslan", "Doğan", "Kılıç", "Aslan", "Çetin", "Kara", "Koç", "Kurt",
        "Özdemir", "Şimşek", "Polat", "Erdoğan", "Aksoy", "Güneş", "Yalçın", "Tekin",
        "Bulut", "Taş", "Acar", "Güler", "Kaplan", "Uçar"
    ];

    private static readonly string[] LeaveReasons =
    [
        "Yıllık izin", "Sağlık raporu", "Evlilik izni", "Doğum izni", "Mazeret izni",
        "Aile ziyareti", "Eğitim", "Taşınma", "Acil durum", "Dinlenme"
    ];

    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Tenants.AnyAsync())
        {
            return;
        }

        var random = new Random(42);
        var now = DateTime.UtcNow;

        foreach (var tenantName in TenantNames)
        {
            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = tenantName,
                CreatedDate = now,
                UpdatedDate = now
            };

            context.Tenants.Add(tenant);

            var departments = CreateDepartments(tenant.Id, now);
            context.Departments.AddRange(departments);

            var skills = CreateSkills(tenant.Id, now);
            context.Skills.AddRange(skills);

            var employees = CreateEmployees(tenant.Id, departments, random, now);
            context.Employees.AddRange(employees);

            var employeeSkills = CreateEmployeeSkills(employees, skills, random);
            context.EmployeeSkills.AddRange(employeeSkills);

            var leaveRequests = CreateLeaveRequests(tenant.Id, employees, random, now);
            context.LeaveRequests.AddRange(leaveRequests);
        }

        await context.SaveChangesAsync();
    }

    private static List<Department> CreateDepartments(Guid tenantId, DateTime now)
    {
        var root = new Department
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = DepartmentNames[0],
            CreatedDate = now,
            UpdatedDate = now
        };

        var departments = new List<Department> { root };

        for (var i = 1; i < DepartmentNames.Length; i++)
        {
            departments.Add(new Department
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Name = DepartmentNames[i],
                ParentDepartmentId = root.Id,
                CreatedDate = now,
                UpdatedDate = now
            });
        }

        return departments;
    }

    private static List<Skill> CreateSkills(Guid tenantId, DateTime now)
    {
        return SkillNames.Select(name => new Skill
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = name,
            CreatedDate = now,
            UpdatedDate = now
        }).ToList();
    }

    private static List<Employee> CreateEmployees(Guid tenantId, List<Department> departments, Random random, DateTime now)
    {
        var employees = new List<Employee>();

        for (var i = 0; i < 30; i++)
        {
            var firstName = FirstNames[i];
            var lastName = LastNames[i];
            var department = departments[random.Next(departments.Count)];

            employees.Add(new Employee
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                DepartmentId = department.Id,
                FirstName = firstName,
                LastName = lastName,
                Email = $"{firstName.ToLowerInvariant()}.{lastName.ToLowerInvariant()}@{tenantId.ToString()[..8]}.com",
                Phone = $"+90 5{random.Next(10, 99)} {random.Next(100, 999)} {random.Next(10, 99)} {random.Next(10, 99)}",
                BirthDate = DateTimeHelper.UtcToday.AddYears(-random.Next(25, 55)).AddDays(-random.Next(0, 365)),
                HireDate = DateTimeHelper.UtcToday.AddYears(-random.Next(1, 10)).AddDays(-random.Next(0, 365)),
                PhotoUrl = null,
                IsActive = random.NextDouble() > 0.15,
                CreatedDate = now,
                UpdatedDate = now
            });
        }

        return employees;
    }

    private static List<EmployeeSkill> CreateEmployeeSkills(List<Employee> employees, List<Skill> skills, Random random)
    {
        var employeeSkills = new List<EmployeeSkill>();

        foreach (var employee in employees)
        {
            var skillCount = random.Next(2, 5);
            var selectedSkills = skills.OrderBy(_ => random.Next()).Take(skillCount);

            foreach (var skill in selectedSkills)
            {
                employeeSkills.Add(new EmployeeSkill
                {
                    EmployeeId = employee.Id,
                    SkillId = skill.Id
                });
            }
        }

        return employeeSkills;
    }

    private static List<LeaveRequest> CreateLeaveRequests(Guid tenantId, List<Employee> employees, Random random, DateTime now)
    {
        var leaveRequests = new List<LeaveRequest>();

        for (var i = 0; i < 20; i++)
        {
            var employee = employees[random.Next(employees.Count)];
            var startOffset = random.Next(-60, 90);
            var duration = random.Next(1, 10);
            var startDate = DateTimeHelper.UtcToday.AddDays(startOffset);
            var endDate = startDate.AddDays(duration);

            leaveRequests.Add(new LeaveRequest
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                EmployeeId = employee.Id,
                StartDate = startDate,
                EndDate = endDate,
                Reason = LeaveReasons[random.Next(LeaveReasons.Length)],
                Status = (LeaveRequestStatus)random.Next(0, 3),
                CreatedDate = now,
                UpdatedDate = now
            });
        }

        return leaveRequests;
    }
}
