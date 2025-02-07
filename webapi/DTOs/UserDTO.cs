using webapi.Models;

namespace webapi.DTOs;

public class UserDTO
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string? Avatar { get; private set; }
    // TODO: Connect with Auth0 and add other properties?

    public UserDTO(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Avatar = user.Avatar;
    }
}

public class UserCollectionDTO
{
    public IEnumerable<UserDTO> Users { get; private set; }

    public UserCollectionDTO(ICollection<User> users)
    {
        Users = users.Select(user => new UserDTO(user));
    }
}
