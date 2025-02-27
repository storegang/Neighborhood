namespace webapi.Models;

public class Category : BaseEntity
{
    // INHERITS: public string Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public string NeighborhoodId { get; set; }
    public Neighborhood Neighborhood { get; set; }
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
