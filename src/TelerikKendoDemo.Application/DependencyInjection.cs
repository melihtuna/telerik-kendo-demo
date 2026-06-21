using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TelerikKendoDemo.Application.DTOs;
using TelerikKendoDemo.Application.Interfaces;
using TelerikKendoDemo.Application.Services;
using TelerikKendoDemo.Application.Validators;

namespace TelerikKendoDemo.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<ILeaveRequestService, LeaveRequestService>();

        services.AddValidatorsFromAssemblyContaining<EmployeeCreateUpdateDtoValidator>();

        return services;
    }
}
