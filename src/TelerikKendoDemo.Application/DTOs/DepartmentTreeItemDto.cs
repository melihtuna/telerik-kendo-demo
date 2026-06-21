namespace TelerikKendoDemo.Application.DTOs;

public class DepartmentTreeItemDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool HasChildren { get; set; }
    public bool Expanded { get; set; }
    public List<DepartmentTreeItemDto> Items { get; set; } = [];
}
