using webapi.Models;

namespace webapi.ViewModels;

public class NeighborhoodViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<CategoryViewModel>? Categories { get; set; }
    public ICollection<UserViewModel>? Users { get; set; }
}
