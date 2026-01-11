namespace ToDoList.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IRepositoryAsync<Category> repository;

    public CategoriesController(IRepositoryAsync<Category> repository)
    {
        this.repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<CategoryGetResponseDto>> Create(CategoryCreateRequestDto request)
    {
        //map to Domain object as soon as possible
        var item = request.ToDomain();

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
            new { CategoryId = item.CategoryId },
            CategoryGetResponseDto.FromDomain(item)); //201
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryGetResponseDto>>> Read()
    {
        IEnumerable<Category> itemsToGet;
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
            : Ok(itemsToGet.Select(CategoryGetResponseDto.FromDomain)); //200
    }

    [HttpGet("{CategoryId:int}")]
    public async Task<ActionResult<CategoryGetResponseDto>> ReadById(int CategoryId)
    {
        //try to retrieve the item by id
        Category? itemToGet;
        try
        {
            itemToGet = await repository.ReadByIdAsync(CategoryId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return (itemToGet is null)
            ? NotFound() //404
            : Ok(CategoryGetResponseDto.FromDomain(itemToGet)); //200
    }

    [HttpPut("{CategoryId:int}")]
    public async Task<IActionResult> UpdateById(int CategoryId, [FromBody] CategoryUpdateRequestDto request)
    {
        //map to Domain object as soon as possible
        var updatedItem = request.ToDomain();
        updatedItem.CategoryId = CategoryId;

        //try to update the item by retrieving it with given id
        try
        {
            //retrieve the item
            var itemToUpdate = await repository.ReadByIdAsync(CategoryId);
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

    [HttpDelete("{CategoryId:int}")]
    public async Task<IActionResult> DeleteById(int CategoryId)
    {
        //try to delete the item
        try
        {
            var itemToDelete = await repository.ReadByIdAsync(CategoryId);
            if (itemToDelete is null)
            {
                return NotFound(); //404
            }

            await repository.DeleteByIdAsync(CategoryId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        //respond to client
        return NoContent(); //204
    }
}
