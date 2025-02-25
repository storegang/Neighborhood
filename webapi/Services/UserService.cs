using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface IUserService
{
    ICollection<User> GetAllUsers();
    User GetUserById(string id);
    void CreateUser(User user);
    void UpdateUser(User user);
    void DeleteUser(string id);
}

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public ICollection<User> GetAllUsers()
    {
        return _userRepository.GetAll();
    }

    public User GetUserById(string id)
    {
        return _userRepository.GetById(id);
    }

    public void CreateUser(User user)
    {
        _userRepository.Add(user);
    }

    public void UpdateUser(User user)
    {
        _userRepository.Update(user);
    }

    public void DeleteUser(string id)
    {
        User user = _userRepository.GetById(id);

        if (user != null)
        {
            _userRepository.Delete(user);
        }
    }
}
