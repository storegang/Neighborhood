using System.Linq.Expressions;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface IBaseService<T> where T : BaseEntity
{
    Task<ICollection<T>> GetAll(Expression<Func<T, object>>[]? includes = null);
    Task<T?> GetById(string id, Expression<Func<T, object>>[]? includes = null);
    Task Create(T entity);
    Task Update(T entity);
    Task Delete(string id);
}

public class BaseService<T>(IGenericRepository<T> repository) : IBaseService<T> where T : BaseEntity
{
    private readonly IGenericRepository<T> _repository = repository;

    public async Task<ICollection<T>> GetAll(Expression<Func<T, object>>[]? includes = null)
    {
        return await _repository.GetAll(includes).ConfigureAwait(false);
    }

    public async Task<T?> GetById(string id, Expression<Func<T, object>>[]? includes = null)
    {
        return await _repository.GetById(id, includes).ConfigureAwait(false);
    }

    public async Task Create(T entity)
    {
        await _repository.Add(entity).ConfigureAwait(false);
    }

    public async Task Update(T entity)
    {
        await _repository.Update(entity).ConfigureAwait(false);
    }

    public async Task Delete(string id)
    {
        var entity = await _repository.GetById(id, null).ConfigureAwait(false);

        if (entity != null)
        {
            await _repository.Delete(entity).ConfigureAwait(false);
        }
    }
}
