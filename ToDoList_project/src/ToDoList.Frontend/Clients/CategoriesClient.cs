namespace ToDoList.Frontend.Clients;

using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Models;

public class CategoriesClient : ICategoriesClient
{
    private readonly HttpClient httpClient;

    public CategoriesClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<CategoryView>> ReadCategoriesAsync()
    {
        var categoryViews = new List<CategoryView>();
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<CategoryGetResponseDto>>("api/Categories");
            if (response is null)
            {
                Console.WriteLine("GET request failed: No categories to read.");
                return categoryViews;
            }
            categoryViews = response.Select(dto => new CategoryView
            {
                Id = dto.Id,
                Name = dto.Name
            }).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occurred: {e.Message}");
        }

        return categoryViews;
    }

    public async Task<CategoryView?> ReadCategoryByIdAsync(int categoryId)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<CategoryGetResponseDto>($"api/Categories/{categoryId}");
            if (response is null)
            {
                Console.WriteLine($"GET request failed: Category with {categoryId} id not found.");
                return null;
            }

            var category = new CategoryView()
            {
                Id = response.Id,
                Name = response.Name
            };
            return category;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occurred: {e.Message}");
            return null;
        }
    }

    public async Task CreateCategoryAsync(CategoryView category)
    {
        try
        {
            var categoryRequest = new CategoryCreateRequestDto(category.Name);
            var response = await httpClient.PostAsJsonAsync("api/Categories", categoryRequest);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("POST request successful: Created new Category.");
            }
            else
            {
                Console.WriteLine($"POST request failed: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occurred: {e.Message}");
        }
    }

    public async Task UpdateCategoryAsync(CategoryView category)
    {
        try
        {
            var categoryRequest = new CategoryUpdateRequestDto(category.Name);
            var response = await httpClient.PutAsJsonAsync($"api/Categories/{category.Id}", categoryRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine($"PUT request successful: Updated Category with id {category.Id}.");
            }
            else
            {
                Console.WriteLine($"PUT request failed: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occurred: {e.Message}");
        }
    }

    public async Task DeleteCategoryAsync(CategoryView category)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"api/Categories/{category.Id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine($"DELETE request successful: Deleted Category with id {category.Id}.");
            }
            else
            {
                Console.WriteLine($"DELETE request failed with status code: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occurred: {e.Message}");
        }
    }
}
