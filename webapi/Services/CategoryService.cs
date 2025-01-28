using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public class CategoryService(ICategoryRepository categoryRepository)
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public ICollection<Category> GetAllCategories()
    {
        return _categoryRepository.GetAll();
    }

    public Category GetCategoryById(int id)
    {
        return _categoryRepository.GetById(id);
    }

    public void CreateCategory(Category category)
    {
        _categoryRepository.Add(category);
    }

    public void UpdateCategory(Category category)
    {
        _categoryRepository.Update(category);
    }

    public void DeleteCategory(int id)
    {
        Category category = _categoryRepository.GetById(id);

        if (category != null)
        {
            _categoryRepository.Delete(category);
        }
    }
}
