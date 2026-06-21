using Microsoft.AspNetCore.Mvc;
using TelerikKendoDemo.Application.DTOs;
using TelerikKendoDemo.Application.Interfaces;

namespace TelerikKendoDemo.Web.Controllers;

public class EmployeesController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;
    private readonly ISkillService _skillService;
    private readonly IWebHostEnvironment _environment;

    public EmployeesController(
        IEmployeeService employeeService,
        IDepartmentService departmentService,
        ISkillService skillService,
        IWebHostEnvironment environment)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
        _skillService = skillService;
        _environment = environment;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Read(CancellationToken cancellationToken)
    {
        var employees = await _employeeService.GetAllAsync(cancellationToken);
        return Json(employees);
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartments(CancellationToken cancellationToken)
    {
        var departments = await _departmentService.GetAllAsync(cancellationToken);
        return Json(departments);
    }

    [HttpGet]
    public async Task<IActionResult> GetSkills(CancellationToken cancellationToken)
    {
        var skills = await _skillService.GetAllAsync(cancellationToken);
        return Json(skills);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var employee = await _employeeService.GetByIdAsync(id, cancellationToken);
        if (employee is null)
        {
            return NotFound(new { success = false, message = "Çalışan bulunamadı." });
        }

        return Json(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeCreateUpdateDto model, CancellationToken cancellationToken)
    {
        var employee = await _employeeService.CreateAsync(model, cancellationToken);
        return Json(new { success = true, message = "Çalışan başarıyla eklendi.", data = employee });
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] EmployeeCreateUpdateDto model, CancellationToken cancellationToken)
    {
        var employee = await _employeeService.UpdateAsync(model, cancellationToken);
        return Json(new { success = true, message = "Çalışan başarıyla güncellendi.", data = employee });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _employeeService.DeleteAsync(id, cancellationToken);
        return Json(new { success = true, message = "Çalışan başarıyla silindi." });
    }

    [HttpPost]
    public async Task<IActionResult> UploadPhoto(IEnumerable<IFormFile> files)
    {
        var file = files.FirstOrDefault();
        if (file is null || file.Length == 0)
        {
            return BadRequest(new { success = false, message = "Dosya seçilmedi." });
        }

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "photos");
        Directory.CreateDirectory(uploadsFolder);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        var photoUrl = $"/uploads/photos/{fileName}";
        return Json(new { success = true, photoUrl });
    }
}
