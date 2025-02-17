namespace webapi.Models;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Avatar { get; set; }
    public string? NeighborhoodId { get; set; }
    // TODO: Connect with Auth0 and add other properties
}
