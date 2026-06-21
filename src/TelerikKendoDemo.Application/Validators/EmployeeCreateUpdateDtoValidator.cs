using FluentValidation;
using TelerikKendoDemo.Application.DTOs;

namespace TelerikKendoDemo.Application.Validators;

public class EmployeeCreateUpdateDtoValidator : AbstractValidator<EmployeeCreateUpdateDto>
{
    public EmployeeCreateUpdateDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad alanı zorunludur.")
            .MaximumLength(100).WithMessage("Ad en fazla 100 karakter olabilir.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad alanı zorunludur.")
            .MaximumLength(100).WithMessage("Soyad en fazla 100 karakter olabilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta alanı zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
            .MaximumLength(200).WithMessage("E-posta en fazla 200 karakter olabilir.");

        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Departman seçimi zorunludur.");

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Today).WithMessage("Doğum tarihi bugünden önce olmalıdır.");

        RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("İşe giriş tarihi bugünden ileri olamaz.");
    }
}
