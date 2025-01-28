using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public class UserService(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public ICollection<User> GetAllUsers()
    {
        return _userRepository.GetAll();
    }

    public User GetUserById(int id)
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

    public void DeleteUser(int id)
    {
        User user = _userRepository.GetById(id);

        if (user != null)
        {
            _userRepository.Delete(user);
        }
    }
}
