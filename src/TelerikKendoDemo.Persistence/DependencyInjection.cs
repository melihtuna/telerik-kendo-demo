using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TelerikKendoDemo.Domain.Interfaces;
using TelerikKendoDemo.Persistence.Context;
using TelerikKendoDemo.Persistence.Repositories;
using TelerikKendoDemo.Persistence.Seed;

namespace TelerikKendoDemo.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();
        await DatabaseSeeder.SeedAsync(context);
    }
}
