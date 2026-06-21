namespace TelerikKendoDemo.Application.Common;

public static class DateTimeHelper
{
    public static DateTime ToUtcDate(DateTime value) =>
        DateTime.SpecifyKind(value.Date, DateTimeKind.Utc);

    public static DateTime UtcToday => DateTime.UtcNow.Date;
}
