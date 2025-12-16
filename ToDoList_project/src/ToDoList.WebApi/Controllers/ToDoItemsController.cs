namespace ToDoList.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

[ApiController]
[Route("api/[controller]")]
public class ToDoItemsController : ControllerBase
{
    private readonly IRepositoryAsync<ToDoItem> repository;
    private readonly IRepositoryAsync<Category> categoryRepository;

    public ToDoItemsController(IRepositoryAsync<ToDoItem> repository, IRepositoryAsync<Category> categoryRepository)
    {
        this.repository = repository;
        this.categoryRepository = categoryRepository;
    }

    [HttpPost]
    public async Task<ActionResult<ToDoItemGetResponseDto>> Create(ToDoItemCreateRequestDto request)
    {
        //map to Domain object as soon as possible
        var item = request.ToDomain();

        // check if category ID exists
        var category = await categoryRepository.ReadByIdAsync(request.CategoryId);
        if (category is null)
        {
            return BadRequest("Category with the specified ID does not exist.");
        }

        //try to create an item
        try
        {
            await repository.CreateAsync(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return CreatedAtAction(
            nameof(ReadById),
            new { toDoItemId = item.ToDoItemId },
            ToDoItemGetResponseDto.FromDomain(item)); //201
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItemGetResponseDto>>> Read()
    {
        IEnumerable<ToDoItem> itemsToGet;
        try
        {
            itemsToGet = await repository.ReadAllAsync();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return (itemsToGet is null)
            ? NotFound() //404
            : Ok(itemsToGet.Select(ToDoItemGetResponseDto.FromDomain)); //200
    }

    [HttpGet("{toDoItemId:int}")]
    public async Task<ActionResult<ToDoItemGetResponseDto>> ReadById(int toDoItemId)
    {
        //try to retrieve the item by id
        ToDoItem? itemToGet;
        try
        {
            itemToGet = await repository.ReadByIdAsync(toDoItemId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return (itemToGet is null)
            ? NotFound() //404
            : Ok(ToDoItemGetResponseDto.FromDomain(itemToGet)); //200
    }

    [HttpPut("{toDoItemId:int}")]
    public async Task<IActionResult> UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        // check if category ID exists
        var category = await categoryRepository.ReadByIdAsync(request.CategoryId);
        if (category is null)
        {
            return BadRequest("Category with the specified ID does not exist.");
        }

        //map to Domain object as soon as possible
        var updatedItem = request.ToDomain();
        updatedItem.ToDoItemId = toDoItemId;

        //try to update the item by retrieving it with given id
        try
        {
            //retrieve the item
            var itemToUpdate = await repository.ReadByIdAsync(toDoItemId);
            if (itemToUpdate is null)
            {
                return NotFound(); //404
            }

            await repository.UpdateAsync(updatedItem);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return NoContent(); //204
    }

    [HttpDelete("{toDoItemId:int}")]
    public async Task<IActionResult> DeleteById(int toDoItemId)
    {
        //try to delete the item
        try
        {
            var itemToDelete = await repository.ReadByIdAsync(toDoItemId);
            if (itemToDelete is null)
            {
                return NotFound(); //404
            }

            await repository.DeleteByIdAsync(toDoItemId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        //respond to client
        return NoContent(); //204
    }
}
