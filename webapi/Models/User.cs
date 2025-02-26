namespace webapi.Models;

public class User : BaseEntity
{
    // INHERITS: public string Id { get; set; }
    public string Name { get; set; }
    public string? Avatar { get; set; }
    public string? NeighborhoodId { get; set; }
    public int UserRole { get; set; }

    public User() { }

    public User(string id = "", string name = "", string? avatar = null, string? neighborhoodId = null, int userRole = 0)
    {
        Id = id;
        Name = name;
        Avatar = avatar;
        NeighborhoodId = neighborhoodId;
        UserRole = userRole;
    }
}
