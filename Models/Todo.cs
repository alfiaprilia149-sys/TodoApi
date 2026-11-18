namespace TodoApi.Models;

public class Todo
{
    public Todo(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public int Id { get; set; }
    public string Title { get; set; } = "";
    public bool IsCompleted { get; set; } = false;
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}