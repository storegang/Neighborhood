using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.ViewModels;
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
    public ActionResult<IEnumerable<CategoryViewModel>> GetAll()
    {
        var categories = _categoryService.GetAllCategories();
        var categoryViewModels = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(categories);
        return Ok(categoryViewModels);
    }

    // GET api/<CategoryController>/5
    [HttpGet("{id}")]
    public ActionResult<CategoryViewModel> GetById(int id)
    {
        var category = _categoryService.GetCategoryById(id);
        
        if (category == null)
        {
            return NotFound();
        }

        var categoryViewModel = _mapper.Map<Category,  CategoryViewModel>(category);
        return Ok(categoryViewModel);
    }

    // POST api/<CategoryController>
    [HttpPost]
    public ActionResult<CategoryViewModel> Create(CategoryViewModel categoryViewModel)
    {
        var category = _mapper.Map<CategoryViewModel, Category>(categoryViewModel);
        _categoryService.CreateCategory(category);

        categoryViewModel.Id = category.Id;

        return CreatedAtAction(nameof(GetById), new { id = categoryViewModel.Id }, categoryViewModel);
    }

    // PUT api/<CategoryController>/5
    [HttpPut("{id}")]
    public IActionResult Update(int id, CategoryViewModel categoryViewModel)
    {
        if (id != int.Parse(categoryViewModel.Id))
        {
            return BadRequest();
        }

        var existingCategory = _categoryService.GetCategoryById(id);
        if (existingCategory == null)
        {
            return NotFound();
        }

        var category = _mapper.Map<CategoryViewModel, Category>(categoryViewModel);
        _categoryService.UpdateCategory(category);

        return NoContent();
    }

    // DELETE api/<CategoryController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
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
