using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface ICategoryService : IBaseService<Category>
{
}

public class CategoryService(IGenericRepository<Category> repository) : BaseService<Category>(repository), ICategoryService
{
}
