using FluentValidation;
using TelerikKendoDemo.Application.DTOs;

namespace TelerikKendoDemo.Application.Validators;

public class LeaveRequestCreateUpdateDtoValidator : AbstractValidator<LeaveRequestCreateUpdateDto>
{
    public LeaveRequestCreateUpdateDtoValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Çalışan seçimi zorunludur.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Başlangıç tarihi zorunludur.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Bitiş tarihi zorunludur.")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("Bitiş tarihi başlangıç tarihinden önce olamaz.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("İzin nedeni zorunludur.")
            .MaximumLength(500).WithMessage("İzin nedeni en fazla 500 karakter olabilir.");
    }
}
