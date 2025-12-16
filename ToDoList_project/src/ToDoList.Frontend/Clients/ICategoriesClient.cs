namespace ToDoList.Frontend.Clients;

using ToDoList.Frontend.Models;

public interface ICategoriesClient
{
    public Task<List<CategoryView>> ReadCategoriesAsync();
    public Task<CategoryView?> ReadCategoryByIdAsync(int categoryId);
    public Task CreateCategoryAsync(CategoryView category);
    public Task UpdateCategoryAsync(CategoryView category);
    public Task DeleteCategoryAsync(CategoryView category);
}