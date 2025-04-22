using webapi.Models;

namespace webapi.DTOs;

public class ClientUserDTO
{
    public string Name { get; set; }
    public string? Avatar { get; set; }

    public ClientUserDTO(){}

    public ClientUserDTO(User user)
    {
        Name = user.Name;
        Avatar = user.Avatar;
    }

    public ClientUserDTO(string name, string? avatar)
    {
        Name = name;
        Avatar = avatar;
    }
}

public class ClientUserCollectionDTO
{
    public IEnumerable<ClientUserDTO> Users { get; set; }

    public ClientUserCollectionDTO(ICollection<User> users)
    {
        Users = users.Select(user => new ClientUserDTO(user));
    }
}
