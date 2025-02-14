namespace webapi.Models;

public class Neighborhood
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<User> Users { get; set; } = new List<User>();
}
