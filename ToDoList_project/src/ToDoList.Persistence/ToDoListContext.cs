namespace ToDoList.Persistence;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoListContext : DbContext
{
    private readonly string connectionString;
    public ToDoListContext(string connectionString = "DataSource=../../data/localdb.db")
    {
        this.connectionString = connectionString;
        Database.Migrate();
    }
    public DbSet<ToDoItem> ToDoItems { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(connectionString);
    }
}
