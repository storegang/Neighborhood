using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using webapi.DataContexts;
using webapi.Models;

namespace webapi.Repositories;

public interface IGenericRepository<T> where T : class
{
    ICollection<T> GetAll(Expression<Func<T, object>>[] include = null);
    T GetById(string id, Expression<Func<T, object>>[] include = null);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly NeighborhoodContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(NeighborhoodContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public ICollection<T> GetAll(Expression<Func<T, object>>[] includes = null)
    {
        IQueryable<T> query = _dbSet;

        if (includes == null)
        {
            return [.. query];
        }
        else
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query?.ToList();
    }

    public T GetById(string id, Expression<Func<T, object>>[] includes = null)
    {
        IQueryable<T> query = _dbSet;

        if (includes == null)
        {
            return query.FirstOrDefault(e => EF.Property<string>(e, "Id") == id);
        }
        else
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query?.FirstOrDefault(e => EF.Property<string>(e, "Id") == id);
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public void Update(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void Delete(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }
}
