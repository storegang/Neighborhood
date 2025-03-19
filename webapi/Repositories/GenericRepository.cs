using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using webapi.DataContexts;
using webapi.Interfaces;
using webapi.Models;

namespace webapi.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<ICollection<T>> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>>[]? includes = null);
    Task<T?> GetById(string id, Func<IQueryable<T>, IIncludableQueryable<T, object>>[]? includes = null);
    Task<T?> GetPaginatedInclude(string id, int page = 0, int pageSize = 5, Func<IQueryable<T>, IIncludableQueryable<T, object>>[]? includes = null);
    Task Add(T entity);
    Task Update(T entity);
    Task Delete(T entity);
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

    public async Task<ICollection<T>> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>>[]? includes = null)
    {
        // In case this is a very large fetch request, we can use CancellationToken to cancel the request
        // Check out "CancellationToken cancellationToken = default" for how to implement this.

        IQueryable<T> query = _dbSet;

        if (includes == null)
        {
            return [.. query];
        }
        else
        {
            foreach (var include in includes)
            {
                query = include(query);
            }
        }

        return await query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<T?> GetById(string id, Func<IQueryable<T>, IIncludableQueryable<T, object>>[]? includes = null)
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
                query = include(query);
            }
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);
    }

    public async Task<T?> GetPaginatedInclude(string id, int page = 0, int pageSize = 5, Func<IQueryable<T>, IIncludableQueryable<T, object>>[]? includes = null)
    {
        _context.ChangeTracker.LazyLoadingEnabled = false;
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = include(query);
            }
        }

        T result = await query.FirstOrDefaultAsync().ConfigureAwait(false);


        return result;
    }

    public async Task Add(T entity)
    {
        if (await _dbSet.FindAsync(entity.Id).ConfigureAwait(false) != null) return;
        await _dbSet.AddAsync(entity).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task Update(T entity)
    {
        var existingEntity = await _context.FindAsync<T>(entity.Id).ConfigureAwait(false);
        if (existingEntity == null) return;
        _dbSet.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task Delete(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
