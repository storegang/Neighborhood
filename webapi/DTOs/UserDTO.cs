using webapi.Models;

namespace webapi.DTOs;

public class UserDTO
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string? Avatar { get; private set; }
    // TODO: Connect with Auth0 and add other properties?
}

public class UsersDTO
{
    public IEnumerable<UserDTO> Users { get; private set; }
}
