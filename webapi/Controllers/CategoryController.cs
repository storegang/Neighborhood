using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using AutoMapper;

namespace webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(CategoryService categoryService, IMapper mapper) : ControllerBase
{
    private readonly CategoryService _categoryService = categoryService;
    private readonly IMapper _mapper = mapper;

    // GET: api/<CategoryController>
    [HttpGet]
    public ActionResult<CategoryCollectionDTO> GetAll()
    {
        var categories = _categoryService.GetAllCategories();
        var categoryViewModels = new CategoryCollectionDTO(categories);
        return Ok(categoryViewModels);
    }

    // GET api/<CategoryController>/5
    [HttpGet("{id}")]
    public ActionResult<CategoryDTO> GetById(string id)
    {
        var category = _categoryService.GetCategoryById(id);
        
        if (category == null)
        {
            return NotFound();
        }

        var categoryViewModel = new CategoryDTO(category);
        return Ok(categoryViewModel);
    }

    // POST api/<CategoryController>
    [HttpPost]
    public ActionResult<CategoryDTO> Create(CategoryDTO categoryViewModel)
    {
        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_categoryService.GetCategoryById(newGuid) != null);

        categoryViewModel.Id = newGuid;
        var category = new Category 
        {
            Id = categoryViewModel.Id,
            Name = categoryViewModel.Name,
            Color = categoryViewModel.Color,
            NeighborhoodId = categoryViewModel.NeighborhoodId
        };
        _categoryService.CreateCategory(category);

        return CreatedAtAction(nameof(GetById), new { id = categoryViewModel.Id }, categoryViewModel);
    }

    // PUT api/<CategoryController>/5
    [HttpPut("{id}")]
    public IActionResult Update(string id, CategoryDTO categoryViewModel)
    {
        var existingCategory = _categoryService.GetCategoryById(id);
        if (existingCategory == null)
        {
            return NotFound();
        }

        categoryViewModel.Id = id;
        var category = new Category
        {
            Id = categoryViewModel.Id,
            Name = categoryViewModel.Name,
            Color = categoryViewModel.Color,
            NeighborhoodId = categoryViewModel.NeighborhoodId
        };
        _categoryService.UpdateCategory(category);

        return NoContent();
    }

    // DELETE api/<CategoryController>/5
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
