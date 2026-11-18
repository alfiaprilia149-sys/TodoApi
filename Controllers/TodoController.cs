using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/todos")]
public class TodosController : ControllerBase
{
    private readonly TodoService _todoService;

    public TodosController(TodoService todoService)
    {
        _todoService = todoService;
    }

    // Mengubah Todo menjadi TodoResponse
    private static TodoResponse ToResponse(Todo todo)
    {
        return new TodoResponse
        {
            Id = todo.Id,
            Title = todo.Title,
            IsCompleted = todo.IsCompleted,
            CategoryId = todo.CategoryId,

            Category = todo.Category == null ? null : new CategoryResponse
            {
                Id = todo.Category.Id,
                Name = todo.Category.Name
            }
        };
    }

    // GET: Mengambil semua Todo dengan filter berdasarkan judul dan kategori
    [HttpGet]
    [HttpGet]
    public async Task<ActionResult<List<TodoResponse>>> GetAll(
    [FromQuery] string? search,
    [FromQuery] int? categoryId)
    {
    var todos = await _todoService.GetAllAsync(
        search,
        categoryId
    );

    var response = todos.Select(ToResponse).ToList();

    return Ok(response);
    }

    // GET: Mengambil Todo berdasarkan ID
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoResponse>> GetById(int id)
    {
        var todo = await _todoService.GetByIdAsync(id);

        if (todo == null)
        {
            return NotFound("Todo tidak ditemukan");
        }

        return Ok(ToResponse(todo));
    }

    // POST: Menambahkan Todo baru
    [HttpPost]
    public async Task<ActionResult<TodoResponse>> Create(
        CreateTodoRequest request)
    {
        // Periksa apakah kategori tersedia
        var categoryExists = await _todoService
            .CategoryExistsAsync(request.CategoryId);

        if (!categoryExists)
        {
            return BadRequest("CategoryId tidak ditemukan");
        }

        var todo = await _todoService.CreateAsync(
            request.Title,
            request.IsCompleted,
            request.CategoryId
        );

        var response = ToResponse(todo);

        return CreatedAtAction(
            nameof(GetById),
            new { id = todo.Id },
            response
        );
    }

    // PUT: Mengubah Todo berdasarkan ID
    [HttpPut("{id}")]
    public async Task<ActionResult<TodoResponse>> Update(
        int id,
        UpdateTodoRequest request)
    {
        var categoryExists = await _todoService
            .CategoryExistsAsync(request.CategoryId);

        if (!categoryExists)
        {
            return BadRequest("CategoryId tidak ditemukan");
        }

        var todo = await _todoService.UpdateAsync(
            id,
            request.Title,
            request.IsCompleted,
            request.CategoryId
        );

        if (todo == null)
        {
            return NotFound("Todo tidak ditemukan");
        }

        return Ok(ToResponse(todo));
    }

    // DELETE: Menghapus Todo berdasarkan ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _todoService.DeleteAsync(id);

        if (!success)
        {
            return NotFound("Todo tidak ditemukan");
        }

        return Ok(new
        {
            message = "Todo dengan ID " + id + " berhasil dihapus"
        });
    }

    // GET: api/todos/category/1
    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<List<TodoResponse>>> GetByCategoryId(
        int categoryId)
    {
        var allTodos = await _todoService.GetAllAsync();

        var todos = allTodos
            .Where(todo => todo.CategoryId == categoryId)
            .ToList();

        var response = todos.Select(ToResponse).ToList();

        return Ok(response);
    }
}