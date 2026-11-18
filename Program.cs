using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Mengaktifkan Controller
builder.Services.AddControllers();

// Mendaftarkan Service
builder.Services.AddScoped<TodoService>();
builder.Services.AddScoped<CategoriesService>();

// Menghubungkan EF Core dengan SQLite
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Mengaktifkan Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Mengaktifkan Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();