using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Services;

public class TodoService
{
    private readonly TodoDbContext _context;

    public TodoService(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CategoryExistsAsync(int? categoryId) // Memeriksa apakah kategori yang akan dicari tersedia
    {
    if (categoryId == null)
    {
        return true;
    }

    return await _context.Categories
        .AnyAsync(c => c.Id == categoryId);
    }

    public async Task<List<Todo>> GetAllAsync( // Memeriksa apakah kategori tersedia & memfilter berdasarkan judul dan kategori
    string? search = null,
    int? categoryId = null)
    {
    var query = _context.Todos
        .Include(t => t.Category)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(search)) // Filter berdasarkan judul
    {
        query = query.Where(t =>
            t.Title.Contains(search));
    }

    if (categoryId.HasValue) // Filter berdasarkan kategori
    {
        query = query.Where(t =>
            t.CategoryId == categoryId);
    }

    return await query.ToListAsync();
    }

    public async Task<List<Todo>> GetAllAsync() // READ: Mengambil semua Todo
    {
        return await _context.Todos
            .Include(t => t.Category)
            .ToListAsync();
    }

    public async Task<Todo?> GetByIdAsync(int id) // READ: Mengambil Todo berdasarkan ID
    {
        return await _context.Todos
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Todo> CreateAsync( // CREATE: Menambahkan Todo
        string title,
        bool isCompleted,
        int? categoryId)
    {
        var todo = new Todo(0, title)
        {
            IsCompleted = isCompleted,
            CategoryId = categoryId
        };

        _context.Todos.Add(todo);

        await _context.SaveChangesAsync();

        return await _context.Todos // Mengambil kembali Todo beserta Category
            .Include(t => t.Category)
            .FirstAsync(t => t.Id == todo.Id);
    }

    public async Task<Todo?> UpdateAsync( // UPDATE: Mengubah Todo
        int id,
        string title,
        bool isCompleted,
        int? categoryId)
    {
        var todo = await GetByIdAsync(id);

        if (todo == null)
        {
            return null;
        }

        todo.Title = title;
        todo.IsCompleted = isCompleted;
        todo.CategoryId = categoryId;

        await _context.SaveChangesAsync();

        return await _context.Todos
            .Include(t => t.Category)
            .FirstAsync(t => t.Id == id);
    }

    public async Task<bool> DeleteAsync(int id) // DELETE: Menghapus Todo
    {
        var todo = await GetByIdAsync(id);

        if (todo == null)
        {
            return false;
        }

        _context.Todos.Remove(todo);

        await _context.SaveChangesAsync();

        return true;
    }
}