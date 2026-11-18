using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs;

public class CreateTodoRequest
{
    [Required(ErrorMessage = "Title tidak boleh kosong")]
    [MinLength(3, ErrorMessage = "Title minimal memiliki 3 karakter")]
    [MaxLength(100, ErrorMessage = "Title maksimal memiliki 100 karakter")]
    public string Title { get; set; } = "";
    public bool IsCompleted { get; set; } = false;
    public int? CategoryId { get; set; }
}