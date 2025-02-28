using System.Linq.Expressions;
using webapi.DataContexts;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface INeighborhoodService : IBaseService<Neighborhood>
{
}

public class NeighborhoodService(IGenericRepository<Neighborhood> repository) : BaseService<Neighborhood>(repository), INeighborhoodService
{
}
