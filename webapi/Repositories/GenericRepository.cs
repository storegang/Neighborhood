using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using webapi.DataContexts;
using webapi.Interfaces;
using webapi.Models;

namespace webapi.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    ICollection<T>? GetAll(Expression<Func<T, object>>[]? includes = null);
    T? GetById(string id, Expression<Func<T, object>>[]? includes = null);
    ICollection<T>? GetPagination(int page, int size = 5, Expression<Func<T, object>>[]? includes = null);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly NeighborhoodContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(NeighborhoodContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public ICollection<T>? GetAll(Expression<Func<T, object>>[]? includes = null)
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

    public T? GetById(string id, Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = _dbSet;

        if (includes == null)
        {
            return query.FirstOrDefault(e => e.Id == id);
        }
        else
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query.FirstOrDefault(e => e.Id == id);
    }

    public ICollection<T>? GetPagination(int page, int size = 5, Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = _dbSet;
        if (includes == null)
        {
            return query.Skip((page - 1) * size).Take(size).ToList();
        }
        else
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query.Skip((page - 1) * size).Take(size).ToList();
    }

    public void Add(T entity)
    {
        if (_dbSet.Find(entity.Id) != null) return;
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public void Update(T entity)
    {
        var existingEntity = _context.Find<T>(entity.Id);
        if (existingEntity == null) return;
        _dbSet.Entry(existingEntity).CurrentValues.SetValues(entity);
        //_context.Entry(entity).State = EntityState.Modified;
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
