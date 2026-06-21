using Serilog;
using TelerikKendoDemo.Application;
using TelerikKendoDemo.Application.Common;
using TelerikKendoDemo.Persistence;
using TelerikKendoDemo.Web.Filters;
using TelerikKendoDemo.Web.Middleware;
using TelerikKendoDemo.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/telerik-kendo-demo-.log", rollingInterval: RollingInterval.Day);
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<TenantActionFilter>();
});

builder.Services.AddKendo();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddApplication();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Veritabanı bağlantı dizesi bulunamadı.");

builder.Services.AddPersistence(connectionString);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

await app.Services.InitializeDatabaseAsync();

app.Run();
