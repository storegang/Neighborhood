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
    Task<ICollection<T>> GetAll(Expression<Func<T, object>>[]? include = null, CancellationToken cancellationToken = default);
    Task<T?> GetById(string id, Expression<Func<T, object>>[]? include = null, CancellationToken cancellationToken = default);
    Task Add(T entity, CancellationToken cancellationToken = default);
    Task Update(T entity, CancellationToken cancellationToken = default);
    Task Delete(T entity, CancellationToken cancellationToken = default);
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

    public async Task<ICollection<T>> GetAll(Expression<Func<T, object>>[]? includes = null, CancellationToken cancellationToken = default)
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

        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<T?> GetById(string id, Expression<Func<T, object>>[]? includes = null, CancellationToken cancellationToken = default)
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

        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task Add(T entity, CancellationToken cancellationToken = default)
    {
        if (await _dbSet.FindAsync(entity, cancellationToken).ConfigureAwait(false) != null) return;
        await _dbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task Update(T entity, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _context.FindAsync<T>(entity.Id, cancellationToken).ConfigureAwait(false);
        if (existingEntity == null) return;
        _dbSet.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task Delete(T entity, CancellationToken cancellationToken = default)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
