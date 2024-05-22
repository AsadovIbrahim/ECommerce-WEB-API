using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IWriteCategoryRepository _writeCategoryRepo;

    public CategoryController(IWriteCategoryRepository writeCategoryRepo)
    {
        _writeCategoryRepo = writeCategoryRepo;
    }

    [HttpPost("AddCategory")]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryVM categoryVM)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = new Category()
        {
            Name = categoryVM.Name,
            Description = categoryVM.Description,
        };

        await _writeCategoryRepo.AddAsync(category);
        await _writeCategoryRepo.SaveChangeAsync();

        return StatusCode(201);
    }
}
