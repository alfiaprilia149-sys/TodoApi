using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs;

public class UpdateCategoryRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Name { get; set; } = "";
}