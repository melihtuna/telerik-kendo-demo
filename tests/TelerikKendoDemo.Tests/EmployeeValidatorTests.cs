using FluentValidation.TestHelper;
using TelerikKendoDemo.Application.DTOs;
using TelerikKendoDemo.Application.Validators;
using TelerikKendoDemo.Domain.Enums;
using TelerikKendoDemo.Application.Mappings;
using TelerikKendoDemo.Domain.Entities;

namespace TelerikKendoDemo.Tests;

public class EmployeeValidatorTests
{
    private readonly EmployeeCreateUpdateDtoValidator _validator = new();

    [Fact]
    public void Gecersiz_Email_Dogrulama_Hatasi_Vermeli()
    {
        var dto = new EmployeeCreateUpdateDto
        {
            FirstName = "Test",
            LastName = "Kullanici",
            Email = "gecersiz-email",
            DepartmentId = Guid.NewGuid(),
            BirthDate = DateTime.Today.AddYears(-30),
            HireDate = DateTime.Today.AddYears(-2)
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void EntityMapper_LeaveRequest_Durum_Metnini_Dogru_Donmeli()
    {
        var leaveRequest = new LeaveRequest
        {
            Status = LeaveRequestStatus.Onaylandi,
            Employee = new Employee { FirstName = "Ali", LastName = "Veli" }
        };

        var dto = leaveRequest.ToDto();

        Assert.Equal("Onaylandı", dto.StatusText);
        Assert.Contains("Ali Veli", dto.Title);
    }
}
