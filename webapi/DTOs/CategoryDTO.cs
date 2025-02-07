using webapi.Models;

namespace webapi.DTOs;

public class CategoryDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }

    public CategoryDTO(Category category)
    {
        Id = category.Id;
        Name = category.Name;
        Color = category.Color;
    }
}

public class CategoryCollectionDTO
{
    public IEnumerable<CategoryDTO> Categories { get; set; }

    public CategoryCollectionDTO(ICollection<Category> categories)
    {
        Categories = categories.Select(category => new CategoryDTO(category));
    }
}
