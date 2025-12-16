namespace ToDoList.Frontend.Models;

using System.ComponentModel.DataAnnotations;

public class CategoryView
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is mandatory.")]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; }
}
