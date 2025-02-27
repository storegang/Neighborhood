using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CategoryController(IGenericService<Category> categoryService, INeighborhoodService neighborhoodService) : ControllerBase
{
    private readonly IGenericService<Category> _categoryService = categoryService;
    private readonly INeighborhoodService _neighborhoodService = neighborhoodService;

    // GET: api/<CategoryController>
    [HttpGet]
    public ActionResult<CategoryCollectionDTO> GetAll()
    {
        var categories = _categoryService.GetAll();
        if (categories == null)
        {
            return NotFound();
        }
        var categoryDataCollection = new CategoryCollectionDTO(categories);
        return Ok(categoryDataCollection);
    }

    // GET api/<CategoryController>/{id}
    [HttpGet("{id}")]
    public ActionResult<CategoryDTO> GetById(string id)
    {
        var category = _categoryService.GetById(id);
        
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
        var neighborhood = _neighborhoodService.GetById(neighborhoodId);

        if (neighborhood == null || neighborhood.Categories == null)
        {
            return NotFound();
        }
        var categories = neighborhood.Categories;

        var categoryData = new CategoryCollectionDTO(categories);
        return Ok(categoryData);
    }

    // POST api/<CategoryController>
    [HttpPost]
    public ActionResult<CategoryDTO> Create(CategoryDTO categoryData)
    {
        var neighborhood = _neighborhoodService.GetById(categoryData.NeighborhoodId);

        if (neighborhood == null)
        {
            return NotFound();
        }
        if (categoryData == null)
        {
            return BadRequest();
        }

        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_categoryService.GetById(newGuid) != null);

        categoryData.Id = newGuid;
        var category = new Category
        {
            Id = categoryData.Id,
            Name = categoryData.Name,
            Color = categoryData.Color,
            NeighborhoodId = categoryData.NeighborhoodId
        };
        _categoryService.Create(category);

        if (neighborhood.Categories == null)
        {
            neighborhood.Categories = new List<Category>();
        }
        neighborhood.Categories.Add(category);
        _neighborhoodService.Update(neighborhood);

        return CreatedAtAction(nameof(GetById), new { id = categoryData.Id }, categoryData);
    }

    // PUT api/<CategoryController>/{id}
    [HttpPut("{id}")]
    public IActionResult Update(string id, CategoryDTO categoryData)
    {
        var existingCategory = _categoryService.GetById(id);
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
        _categoryService.Update(category);

        return NoContent();
    }

    // DELETE api/<CategoryController>/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existingCategory = _categoryService.GetById(id);
        if (existingCategory == null)
        {
            return NotFound();
        }

        _categoryService.Delete(id);

        return NoContent();
    }
}
