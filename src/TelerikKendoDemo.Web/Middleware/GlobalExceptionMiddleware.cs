using System.Net;
using System.Text.Json;
using FluentValidation;
using TelerikKendoDemo.Application.Common;

namespace TelerikKendoDemo.Web.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "İşlem sırasında beklenmeyen bir hata oluştu.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            NotFoundException notFound => (HttpStatusCode.NotFound, notFound.Message),
            BusinessException appEx => (HttpStatusCode.BadRequest, appEx.Message),
            ValidationException validation => (HttpStatusCode.BadRequest, string.Join(" ", validation.Errors.Select(e => e.ErrorMessage))),
            _ => (HttpStatusCode.InternalServerError, "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            success = false,
            message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
