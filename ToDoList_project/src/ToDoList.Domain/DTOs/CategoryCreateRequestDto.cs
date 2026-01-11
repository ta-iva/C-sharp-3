namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record CategoryCreateRequestDto(string Name)
{
    public Category ToDomain() => new() { Name = Name };
}