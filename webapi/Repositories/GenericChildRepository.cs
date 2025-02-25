using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using webapi.DataContexts;
using webapi.Models;

namespace webapi.Repositories;

public interface IGenericChildRepository<T> where T : class
{
    public ICollection<T> GetByIdWithChildren(string id, Expression<Func<T, object>> include);
    public T GetByIdWithMultipleIncludes(string id, Expression<Func<T, object>>[] includes);
}

public class GenericChildRepository<T> : IGenericChildRepository<T> where T : class
{
    private readonly NeighborhoodContext _context;
    private readonly DbSet<T> _dbSet;
    public GenericChildRepository(NeighborhoodContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public ICollection<T> GetByIdWithChildren(string id, Expression<Func<T, object>> include)
    {
        IQueryable<T> query = _dbSet;

        query = query.Include(include);

        return (ICollection<T>)query.FirstOrDefault(e => EF.Property<string>(e, "Id") == id);
    }

    public T GetByIdWithMultipleIncludes(string id, Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query.FirstOrDefault(e => EF.Property<string>(e, "Id") == id);
    }
}
