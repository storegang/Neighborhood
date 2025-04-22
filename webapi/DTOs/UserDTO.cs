using Microsoft.AspNetCore.Identity;
using webapi.Models;

namespace webapi.DTOs;

public class UserDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Avatar { get; set; }
    public string? NeighborhoodId { get; set; }
    public IEnumerable<string>? Roles { get; set; }

    public UserDTO(){}

    public UserDTO(User user, IEnumerable<string>? roles = null)
    {
        Id = user.Id;
        Name = user.Name;
        Avatar = user.Avatar;
        NeighborhoodId = user.NeighborhoodId;
        Roles = roles;
    }

    public UserDTO(string id, string name, string? avatar, string neighborhoodId, IEnumerable<string>? roles = null)
    {
        Id = id;
        Name = name;
        Avatar = avatar;
        NeighborhoodId = neighborhoodId;
        Roles = roles;
    }
}

public class UserCollectionDTO
{
    public IEnumerable<UserDTO> Users { get; set; }

    public UserCollectionDTO(ICollection<User> users)
    {
        Users = users.Select(user => new UserDTO(user));
    }
}
