namespace ToDoList.Domain.Models;

using System.ComponentModel.DataAnnotations;

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    [StringLength(50, MinimumLength = 1)]
    public required string Name { get; set; }
    public ICollection<ToDoItem> ToDoItems { get; set; } = []; // Navigation property
}
