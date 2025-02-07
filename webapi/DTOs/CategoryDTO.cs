using webapi.Models;

namespace webapi.DTOs;

public class CategoryDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
}

public class CategoriesDTO
{
    public IEnumerable<CategoryDTO> Categories { get; set; }
}
