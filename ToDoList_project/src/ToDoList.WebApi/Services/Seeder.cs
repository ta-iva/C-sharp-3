using System.Text.Json;
using ToDoList.Domain.Models;
using ToDoList.Persistence;

namespace ToDoList.WebApi.Services;

public class Seeder
{
    private readonly ToDoListContext _context;

    public Seeder(ToDoListContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (_context.Categories.Any() || _context.ToDoItems.Any())
        {
            return; // Already seeded
        }

        // Load categories
        var categoriesJson = await File.ReadAllTextAsync("../../data/categories.json");
        var categories = JsonSerializer.Deserialize<List<Category>>(categoriesJson);
        if (categories != null)
        {
            _context.Categories.AddRange(categories);
        }

        // Load todoitems
        var todoItemsJson = await File.ReadAllTextAsync("../../data/todoitems.json");
        var todoItems = JsonSerializer.Deserialize<List<ToDoItem>>(todoItemsJson);
        if (todoItems != null)
        {
            _context.ToDoItems.AddRange(todoItems);
        }

        await _context.SaveChangesAsync();
    }
}
