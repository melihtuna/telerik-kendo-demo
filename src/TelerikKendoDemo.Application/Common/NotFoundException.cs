namespace TelerikKendoDemo.Application.Common;

public class NotFoundException : BusinessException
{
    public NotFoundException(string message) : base(message)
    {
    }
}
