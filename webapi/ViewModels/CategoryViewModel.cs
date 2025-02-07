using webapi.Models;

namespace webapi.ViewModels;

public class CategoryViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }

    public string NeighborhoodId { get; set; }
    public ICollection<PostViewModel>? Posts { get; set; }
}
