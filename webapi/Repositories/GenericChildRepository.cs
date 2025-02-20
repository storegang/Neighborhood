using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using webapi.DataContexts;
using webapi.Models;

namespace webapi.Repositories;

public interface IGenericChildRepository<T1> where T1 : class
{
    ICollection<T1> GetByIdWithChildren(string id);
}

public class GenericChildRepository<T1> : IGenericChildRepository<T1> where T1 : class
{
    private readonly NeighborhoodContext _context;
    private readonly DbSet<T1> _dbSet;
    private readonly Dictionary<Type, Func<string, IQueryable<T1>>> _includeExpressions;
    public GenericChildRepository(NeighborhoodContext context)
    {
        _context = context;
        _dbSet = _context.Set<T1>();
        _includeExpressions = new Dictionary<Type, Func<string, IQueryable<T1>>>
        {
            { typeof(Neighborhood), id => _dbSet.Include(n => ((Neighborhood)(object)n).Categories).Where(n => ((Neighborhood)(object)n).Id == id).Cast<T1>() },
            { typeof(Category), id => _dbSet.Include(c => ((Category)(object)c).Posts).Where(c => ((Category)(object)c).Id == id).Cast<T1>() },
            { typeof(Post), id => _dbSet.Include(p => ((Post)(object)p).Comments).Where(p => ((Post)(object)p).Id == id).Cast<T1>() },
        };
    }
    public ICollection<T1> GetByIdWithChildren(string id)
    {
        if (_includeExpressions.TryGetValue(typeof(T1), out var includeExpression))
        {
            return includeExpression(id).ToList();
        }

        return new List<T1>();
    }
}
