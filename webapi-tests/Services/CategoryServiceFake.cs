using webapi.DTOs;
using webapi.Models;
using webapi.Services;

namespace webapi.Tests.Services;

public class CategoryServiceFake : ICategoryService
{
    private readonly List<Category> _categoryRepository;

    public CategoryServiceFake()
    {
        _categoryRepository = new List<Category>()
                {
                    new Category()
                    {
                        Id = "1",
                        Name = "Category 1",
                        Color = "#000000",
                        NeighborhoodId = "1",
                    },
                    new Category()
                    {
                        Id = "2",
                        Name = "Category 2",
                        Color = "#FFFFFF",
                        NeighborhoodId = "1",
                        Posts = new List<Post>()
                        {
                            new Post()
                            {
                                Id = "3",
                                Title = "Post 1",
                                Description = "Content 1",
                                CategoryId = "3"
                            },
                            new Post()
                            {
                                Id = "4",
                                Title = "Post 2",
                                Description = "Content 2",
                                CategoryId = "3"
                            }
                        }
                    },
                    new Category()
                    {
                        Id = "3",
                        Name = "Category 3",
                        Color = "#0000FF",
                        NeighborhoodId = "2",
                        Posts = new List<Post>()
                        {
                            new Post()
                            {
                                Id = "5",
                                Title = "Post 1",
                                Description = "Content 1",
                                CategoryId = "3"
                            },
                            new Post()
                            {
                                Id = "6",
                                Title = "Post 2",
                                Description = "Content 2",
                                CategoryId = "3"
                            }
                        }
                    }
                };
    }

    public ICollection<Category> GetAllCategories()
    {
        return _categoryRepository;
    }

    public Category GetCategoryById(string id)
    {
        return _categoryRepository.Where(c => c.Id == id).FirstOrDefault();
    }

    public Category GetCategoryByIdWithChildren(string id)
    {
        return _categoryRepository.Where(c => c.Id == id).FirstOrDefault();
    }

    public void CreateCategory(Category category)
    {
        _categoryRepository.Add(category);
    }

    public void UpdateCategory(Category category)
    {
        Category oldCategory = _categoryRepository.Where(c => c.Id == category.Id).FirstOrDefault();

        if (oldCategory != null)
        {
            _categoryRepository.Remove(oldCategory);
        }

        oldCategory = category;
    }

    public void DeleteCategory(string id)
    {
        Category category = _categoryRepository.Where(c => c.Id == id).FirstOrDefault();

        if (category != null)
        {
            _categoryRepository.Remove(category);
        }
    }
}
