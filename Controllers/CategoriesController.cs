using Microsoft.AspNetCore.Mvc;
using TodoApi.Services;
using TodoApi.DTOs;

namespace TodoApi.Controllers; 

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CategoriesService _categoryService;

    public CategoriesController(CategoriesService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();

        var response = categories.Select(category => new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name
        }).ToList();

        return Ok(response);
    }

    // GET: api/categories/1
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponse>> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound("Kategori tidak ditemukan");
        }

        var response = new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name
        };

        return Ok(response);
    }

    // POST: api/categories
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Create(
        CreateCategoryRequest request)
    {
        var category = await _categoryService.CreateAsync(request.Name);

        var response = new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name
        };

        return CreatedAtAction(nameof(GetAll), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryResponse>> Update(int id, UpdateCategoryRequest request)
    {
        var category = await _categoryService.UpdateAsync(id, request.Name);

        if (category == null)
        {
            return NotFound("Kategori tidak ditemukan");
        }

        var response = new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _categoryService.DeleteAsync(id);

        if (!success)
        {
            return NotFound("Kategori tidak ditemukan");
        }

        return Ok(new { message = "Kategori dengan ID " + id + " berhasil dihapus" });
    }
}