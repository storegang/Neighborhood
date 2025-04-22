using Microsoft.AspNetCore.Identity;
using webapi.DTOs;

namespace webapi.Models;

public class User : IdentityUser
{
    public string Name { get; set; }
    public string? Avatar { get; set; }
    public string? NeighborhoodId { get; set; }

    public User() { }

    public User(ServerUserDTO userDTO)
    {
        Id = userDTO.Id;
        Name = userDTO.Name;
        Avatar = userDTO.Avatar;
        NeighborhoodId = userDTO.NeighborhoodId;
    }

    public User(string id, string name, string? avatar = null, string? neighborhoodId = null)
    {
        Id = id;
        Name = name;
        Avatar = avatar;
        NeighborhoodId = neighborhoodId;
    }
}
