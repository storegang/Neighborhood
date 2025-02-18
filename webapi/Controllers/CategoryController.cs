using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CategoryController(CategoryService categoryService, NeighborhoodService neighborhoodService) : ControllerBase
{
    private readonly CategoryService _categoryService = categoryService;
    private readonly NeighborhoodService _neighborhoodService = neighborhoodService;

    // GET: api/<CategoryController>
    [HttpGet]
    public ActionResult<CategoryCollectionDTO> GetAll()
    {
        var categories = _categoryService.GetAllCategories();
        var categoryDataCollection = new CategoryCollectionDTO(categories);
        return Ok(categoryDataCollection);
    }

    // GET api/<CategoryController>/{id}
    [HttpGet("{id}")]
    public ActionResult<CategoryDTO> GetById(string id)
    {
        var category = _categoryService.GetCategoryById(id);
        
        if (category == null)
        {
            return NotFound();
        }

        var categoryData = new CategoryDTO(category);
        return Ok(categoryData);
    }

    // GET api/<CategoryController>/FromNeighborhood={neighborhoodId}
    [HttpGet("FromNeighborhood={neighborhoodId}")]
    public ActionResult<CategoryCollectionDTO> GetCategoryByNeighborhoodId(string neighborhoodId)
    {
        var neighborhood = _neighborhoodService.GetNeighborhoodByIdWithChildren(neighborhoodId);
        var categories = neighborhood.Categories;

        if (neighborhood == null || neighborhood.Categories == null)
        {
            return NotFound();
        }

        var categoryData = new CategoryCollectionDTO(categories);
        return Ok(categoryData);
    }

    // POST api/<CategoryController>
    [HttpPost]
    public ActionResult<CategoryDTO> Create(CategoryDTO categoryData)
    {
        var neighborhood = _neighborhoodService.GetNeighborhoodById(categoryData.NeighborhoodId);

        if (neighborhood == null)
        {
            return NotFound();
        }

        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_categoryService.GetCategoryById(newGuid) != null);

        categoryData.Id = newGuid;
        var category = new Category 
        {
            Id = categoryData.Id,
            Name = categoryData.Name,
            Color = categoryData.Color,
            NeighborhoodId = categoryData.NeighborhoodId
        };
        _categoryService.CreateCategory(category);

        neighborhood.Categories.Add(category);
        _neighborhoodService.UpdateNeighborhood(neighborhood);

        return CreatedAtAction(nameof(GetById), new { id = categoryData.Id }, categoryData);
    }

    // PUT api/<CategoryController>/{id}
    [HttpPut("{id}")]
    public IActionResult Update(string id, CategoryDTO categoryData)
    {
        var existingCategory = _categoryService.GetCategoryById(id);
        if (existingCategory == null)
        {
            return NotFound();
        }

        categoryData.Id = id;
        var category = new Category
        {
            Id = categoryData.Id,
            Name = categoryData.Name,
            Color = categoryData.Color,
            NeighborhoodId = categoryData.NeighborhoodId
        };
        _categoryService.UpdateCategory(category);

        return NoContent();
    }

    // DELETE api/<CategoryController>/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existingCategory = _categoryService.GetCategoryById(id);
        if (existingCategory == null)
        {
            return NotFound();
        }

        _categoryService.DeleteCategory(id);

        return NoContent();
    }
}
