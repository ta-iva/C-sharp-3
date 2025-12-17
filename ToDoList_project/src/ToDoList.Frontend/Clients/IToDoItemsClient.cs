namespace ToDoList.Frontend.Clients;

using ToDoList.Frontend.Models;

public interface IToDoItemsClient
{
    public Task<List<ToDoItemView>> ReadItemsAsync();
    public Task<ToDoItemView?> ReadItemByIdAsync(int itemId);
    public Task CreateItemAsync(ToDoItemView item);
    public Task UpdateItemAsync(ToDoItemView item);
    public Task DeleteItemAsync(ToDoItemView itemView);
}
