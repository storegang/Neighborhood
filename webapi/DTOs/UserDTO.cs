using webapi.Models;

namespace webapi.DTOs;

public class UserDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Avatar { get; set; }
    // TODO: Connect with Auth0 and add other properties?

    public UserDTO(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Avatar = user.Avatar;
    }

    public UserDTO(string id, string name, string? avatar)
    {
        Id = id;
        Name = name;
        Avatar = avatar;
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
