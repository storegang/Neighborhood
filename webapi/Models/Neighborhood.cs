namespace webapi.Models;

public class Neighborhood
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Category>? Categories { get; set; }
    public ICollection<User>? Users { get; set; }
}
