using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface IUserService : IBaseService<User>
{
}

public class UserService(IGenericRepository<User> repository) : BaseService<User>(repository), IUserService
{
}
