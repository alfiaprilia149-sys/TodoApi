using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Data;

namespace TodoApi.Services;

public class CategoriesService
{
    private readonly TodoDbContext _context;

    public CategoriesService(TodoDbContext context)
    {
        _context = context;
    }

    // GET: Mengambil semua kategori
    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    // GET: Mengambil kategori berdasarkan ID
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    // CREATE: Menambahkan kategori baru
    public async Task<Category> CreateAsync(string name)
    {
        var category = new Category
        {
            Name = name
        };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    // UPDATE: Mengubah kategori
    public async Task<Category?> UpdateAsync(int id, string name)
    {
        var category = await GetByIdAsync(id);

        if (category == null)
        {
            return null;
        }

        category.Name = name;
        await _context.SaveChangesAsync();

        return category;
    }

    // DELETE: Menghapus kategori
    public async Task<bool> DeleteAsync(int id)
    {
        var category = await GetByIdAsync(id);

        if (category == null)
        {
            return false;
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return true;
    }
}