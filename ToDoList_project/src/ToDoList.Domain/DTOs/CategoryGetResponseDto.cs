namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record class CategoryGetResponseDto(int Id, string Name)
{
    public static CategoryGetResponseDto FromDomain(Category category) => new(category.CategoryId, category.Name);
}