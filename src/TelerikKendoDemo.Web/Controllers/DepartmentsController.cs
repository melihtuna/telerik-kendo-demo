using Microsoft.AspNetCore.Mvc;
using TelerikKendoDemo.Application.Interfaces;

namespace TelerikKendoDemo.Web.Controllers;

public class DepartmentsController : Controller
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ReadTree(Guid? id, CancellationToken cancellationToken)
    {
        var tree = await _departmentService.GetTreeAsync(id, cancellationToken);
        return Json(tree);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DepartmentCreateModel model, CancellationToken cancellationToken)
    {
        var department = await _departmentService.CreateAsync(model.Name, model.ParentDepartmentId, cancellationToken);
        return Json(new { success = true, message = "Departman başarıyla eklendi.", data = department });
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] DepartmentUpdateModel model, CancellationToken cancellationToken)
    {
        var department = await _departmentService.UpdateAsync(model.Id, model.Name, cancellationToken);
        return Json(new { success = true, message = "Departman başarıyla güncellendi.", data = department });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _departmentService.DeleteAsync(id, cancellationToken);
        return Json(new { success = true, message = "Departman başarıyla silindi." });
    }

    public class DepartmentCreateModel
    {
        public string Name { get; set; } = string.Empty;
        public Guid? ParentDepartmentId { get; set; }
    }

    public class DepartmentUpdateModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
