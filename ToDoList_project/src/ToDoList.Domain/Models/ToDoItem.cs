namespace ToDoList.Domain.Models;

using System.ComponentModel.DataAnnotations;

public class ToDoItem
{
    [Key]
    public int ToDoItemId { get; set; } // EF core looks for <id> nebo <nameId>
    [StringLength(50, MinimumLength = 1)]
    public required string Name { get; set; }
    [StringLength(250)]
    public required string Description { get; set; }
    public bool IsCompleted { get; set; }
    public int CategoryId { get; set; } // Foreign key
    public Category Category { get; set; } // Navigation property
}
