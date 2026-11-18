namespace TodoApi.DTOs;

public class TodoResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public bool IsCompleted { get; internal set; }
    public int? CategoryId { get; set; }
    public CategoryResponse? Category { get; set; }
}