using Microsoft.AspNetCore.Identity;
using webapi.Models;

namespace webapi.DTOs;

public class ServerUserDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Avatar { get; set; }
    public string? NeighborhoodId { get; set; }
    public IEnumerable<string>? Roles { get; set; }

    public ServerUserDTO(){}

    public ServerUserDTO(User user, IEnumerable<string>? roles = null)
    {
        Id = user.Id;
        Name = user.Name;
        Avatar = user.Avatar;
        NeighborhoodId = user.NeighborhoodId;
        Roles = roles;
    }

    public ServerUserDTO(string id, string name, string? avatar, string neighborhoodId, IEnumerable<string>? roles = null)
    {
        Id = id;
        Name = name;
        Avatar = avatar;
        NeighborhoodId = neighborhoodId;
        Roles = roles;
    }
}

public class ServerUserCollectionDTO
{
    public IEnumerable<ServerUserDTO> Users { get; set; }

    public ServerUserCollectionDTO(ICollection<User> users)
    {
        Users = users.Select(user => new ServerUserDTO(user));
    }
}
