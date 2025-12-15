namespace ToDoList.Persistence.Repositories;

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class CategoryRepository : IRepositoryAsync<Category>
{
    private readonly ToDoItemsContext context;

    public CategoryRepository(ToDoItemsContext context)
    {
        this.context = context;
    }

    public async Task CreateAsync(Category item)
    {
        await context.Categories.AddAsync(item);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Category>> ReadAllAsync() => await context.Categories.ToListAsync();

    public async Task<Category?> ReadByIdAsync(int id) => await context.Categories.FindAsync(id);

    public async Task UpdateAsync(Category item)
    {
        var foundItem = await context.Categories.FindAsync(item.CategoryId) ?? throw new ArgumentOutOfRangeException($"Category with ID {item.CategoryId} not found.");
        context.Entry(foundItem).CurrentValues.SetValues(item);
        await context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var item = await context.Categories.FindAsync(id) ?? throw new ArgumentOutOfRangeException($"Category with ID {id} not found.");
        context.Categories.Remove(item);
        await context.SaveChangesAsync();
    }
}
