using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Text.Json;

namespace ECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IReadProductRepository _readProductRepo;
    private readonly IWriteProductRepository _writeProductRepo;

    public ProductController(IReadProductRepository readProductRepo, IWriteProductRepository writeProductRepo)
    {
        _readProductRepo = readProductRepo;
        _writeProductRepo = writeProductRepo;
    }

    [HttpGet("AllProducts")]
    public async Task<IActionResult> GetAll([FromQuery]PaginationVM paginationVM)
    {
        var products = await _readProductRepo.GetAllAsync();
        var prodcutForPage = products.ToList().
                    Skip(paginationVM.Page*paginationVM.PageSize).Take(paginationVM.PageSize).ToList();


        var allProductVm = prodcutForPage.Select(p => new AllProductVM()
        {
            Id=p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            CategoryName = p.Category.Name,
            ImageUrl = p.ImageUrl,
            Stock = p.Stock
        }).ToList();

        return Ok(allProductVm);
    }

    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([FromBody] AddProductVM productVM)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = new Product()
        {
            Name = productVM.Name,
            Price = productVM.Price,
            Description = productVM.Description,
            CategoryId = productVM.CategoryId,
        };

        await _writeProductRepo.AddAsync(product);
        await _writeProductRepo.SaveChangeAsync();

        return StatusCode(201);
    }

    [HttpDelete("DeleteProduct/{id}")]
    public async Task<IActionResult>DeleteProduct(int id)
    {
        var data = await _readProductRepo.GetByIdAsync(id);
        if (data == null)
        {
            return NotFound();
        }
        await _writeProductRepo.DeleteAsync(data);
        await _writeProductRepo.SaveChangeAsync();

        return Ok();
    }
    [HttpPut("UpdateProduct/{id}")]
    public async Task<IActionResult>UpdateProduct(int id, [FromBody]UpdateProductVM updateProductVM)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var product = await _readProductRepo.GetByIdAsync(id);
        if(product== null)
        {
            return NotFound();
        }
        var category=await _readProductRepo.GetByIdAsync(updateProductVM.CategoryId);
        if (category == null)
        {
            return NotFound("Category Not Found!");
        }
        product.Name = updateProductVM.Name;
        product.Price = updateProductVM.Price;
        product.Description = updateProductVM.Description;
        product.CategoryId= updateProductVM.CategoryId;

        await _writeProductRepo.UpdateAsync(product);
        await _writeProductRepo.SaveChangeAsync();

        return Ok();


    }
}
