using System.Linq.Expressions;
using webapi.DataContexts;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface INeighborhoodService : IGenericService<Neighborhood>
{
}

public class NeighborhoodService(IGenericService<Neighborhood> genericService) : INeighborhoodService
{
    private readonly IGenericService<Neighborhood> _genericService = genericService;

    public ICollection<Neighborhood>? GetAll(Expression<Func<Neighborhood, object>>[]? includes = null, Expression<Func<Neighborhood, object>>[]? thenInclude = null)
    {
        return _genericService.GetAll(includes, thenInclude);
    }

    public Neighborhood? GetById(string id, Expression<Func<Neighborhood, object>>[]? includes = null, Expression<Func<Neighborhood, object>>[]? thenInclude = null)
    {
        return _genericService.GetById(id, includes, thenInclude);
    }

    public void Create(Neighborhood entity)
    {
        _genericService.Create(entity);
    }

    public void Update(Neighborhood entity)
    {
        _genericService.Update(entity);
    }

    public void Delete(string id)
    {
        _genericService.Delete(id);
    }
}
