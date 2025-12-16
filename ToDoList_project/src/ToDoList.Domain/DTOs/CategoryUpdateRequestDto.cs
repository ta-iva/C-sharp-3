namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record CategoryUpdateRequestDto(string Name)
{
    public Category ToDomain() => new() { Name = Name };
}
