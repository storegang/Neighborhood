using webapi.Models;

namespace webapi.ViewModels;

public class UserViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Avatar { get; set; }
    // TODO: Connect with Auth0 and add other properties
}
