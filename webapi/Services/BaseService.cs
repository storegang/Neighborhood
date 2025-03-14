using System.Linq.Expressions;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface IBaseService<T> where T : BaseEntity
{
    Task<ICollection<T>> GetAll(Expression<Func<T, object>>[]? includes = null, CancellationToken cancellationToken = default);
    Task<T?> GetById(string id, Expression<Func<T, object>>[]? includes = null, CancellationToken cancellationToken = default);
    Task Create(T entity, CancellationToken cancellationToken = default);
    Task Update(T entity, CancellationToken cancellationToken = default);
    Task Delete(string id, CancellationToken cancellationToken = default);
}

public class BaseService<T>(IGenericRepository<T> repository) : IBaseService<T> where T : BaseEntity
{
    private readonly IGenericRepository<T> _repository = repository;

    public async Task<ICollection<T>> GetAll(Expression<Func<T, object>>[]? includes = null, CancellationToken cancellationToken = default)
    {
        return await _repository.GetAll(includes, cancellationToken).ConfigureAwait(false);
    }

    public async Task<T?> GetById(string id, Expression<Func<T, object>>[]? includes = null, CancellationToken cancellationToken = default)
    {
        return await _repository.GetById(id, includes, cancellationToken).ConfigureAwait(false);
    }

    public async Task Create(T entity, CancellationToken cancellationToken = default)
    {
        await _repository.Add(entity, cancellationToken).ConfigureAwait(false);
    }

    public async Task Update(T entity, CancellationToken cancellationToken = default)
    {
        await _repository.Update(entity, cancellationToken).ConfigureAwait(false);
    }

    public async Task Delete(string id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetById(id, null, cancellationToken).ConfigureAwait(false);

        if (entity != null)
        {
            await _repository.Delete(entity, cancellationToken).ConfigureAwait(false);
        }
    }
}
