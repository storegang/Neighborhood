using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CategoryController(IBaseService<Category> categoryService, INeighborhoodService neighborhoodService) : ControllerBase
{
    private readonly IBaseService<Category> _categoryService = categoryService;
    private readonly INeighborhoodService _neighborhoodService = neighborhoodService;

    // GET: api/<CategoryController>
    [HttpGet]
    public async Task<ActionResult<CategoryCollectionDTO>> GetAll()
    {
        ICollection<Category>? categories = await _categoryService.GetAll();
        CategoryCollectionDTO categoryDataCollection = new CategoryCollectionDTO(categories);
        return Ok(categoryDataCollection);
    }

    // GET api/<CategoryController>/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDTO>> GetById(string id)
    {
        Category? category = await _categoryService.GetById(id);
        
        if (category == null)
        {
            return NotFound();
        }

        CategoryDTO categoryData = new CategoryDTO(category);
        return Ok(categoryData);
    }

    // GET api/<CategoryController>/FromNeighborhood={neighborhoodId}
    [HttpGet("FromNeighborhood={neighborhoodId}")]
    public async Task<ActionResult<CategoryCollectionDTO>> GetCategoryByNeighborhoodId(string neighborhoodId)
    {
        Neighborhood? neighborhood = await _neighborhoodService.GetById(neighborhoodId);

        if (neighborhood == null || neighborhood.Categories == null)
        {
            return NotFound();
        }
        ICollection<Category> categories = neighborhood.Categories;

        CategoryCollectionDTO categoryData = new CategoryCollectionDTO(categories);
        return Ok(categoryData);
    }

    // POST api/<CategoryController>
    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> Create(CategoryDTO categoryData)
    {
        Neighborhood? neighborhood = await _neighborhoodService.GetById(categoryData.NeighborhoodId);

        if (neighborhood == null)
        {
            return NotFound();
        }

        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_categoryService.GetById(newGuid) != null);

        categoryData.Id = newGuid;
        Category category = new Category
        {
            Id = categoryData.Id,
            Name = categoryData.Name,
            Color = categoryData.Color,
            NeighborhoodId = categoryData.NeighborhoodId
        };
        await _categoryService.Create(category);

        if (neighborhood.Categories == null)
        {
            neighborhood.Categories = new List<Category>();
        }
        neighborhood.Categories.Add(category);
        await _neighborhoodService.Update(neighborhood);

        return CreatedAtAction(nameof(GetById), new { id = categoryData.Id }, categoryData);
    }

    // PUT api/<CategoryController>/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, CategoryDTO categoryData)
    {
        Category? existingCategory = await _categoryService.GetById(id);
        if (existingCategory == null)
        {
            return NotFound();
        }

        categoryData.Id = id;
        Category category = new Category
        {
            Id = categoryData.Id,
            Name = categoryData.Name,
            Color = categoryData.Color,
            NeighborhoodId = categoryData.NeighborhoodId
        };
        await _categoryService.Update(category);

        return NoContent();
    }

    // DELETE api/<CategoryController>/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        Category? existingCategory = await _categoryService.GetById(id);
        if (existingCategory == null)
        {
            return NotFound();
        }

        await _categoryService.Delete(id);

        return NoContent();
    }
}
