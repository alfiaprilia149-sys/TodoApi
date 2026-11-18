using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs;

public class CreateCategoryRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Name { get; set; } = "";
}