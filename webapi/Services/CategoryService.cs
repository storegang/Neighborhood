﻿using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface ICategoryService
{
    ICollection<Category> GetAllCategories();
    Category GetCategoryById(string id);
    Category GetCategoryByIdWithChildren(string id);
    void CreateCategory(Category category);
    void UpdateCategory(Category category);
    void DeleteCategory(string id);
}

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public ICollection<Category> GetAllCategories()
    {
        return _categoryRepository.GetAll();
    }

    public Category GetCategoryById(string id)
    {
        return _categoryRepository.GetById(id);
    }

    public Category GetCategoryByIdWithChildren(string id)
    {
        return _categoryRepository.GetByIdWithChildren(id);
    }

    public void CreateCategory(Category category)
    {
        _categoryRepository.Add(category);
    }

    public void UpdateCategory(Category category)
    {
        _categoryRepository.Update(category);
    }

    public void DeleteCategory(string id)
    {
        Category category = _categoryRepository.GetById(id);

        if (category != null)
        {
            _categoryRepository.Delete(category);
        }
    }
}
