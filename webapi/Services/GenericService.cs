using System.Linq.Expressions;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface IGenericService<T> where T : BaseEntity
{
    ICollection<T>? GetAll(Expression<Func<T, object>>[] includes = null);
    T? GetById(string id, Expression<Func<T, object>>[] includes = null);
    void Create(T entity);
    void Update(T entity);
    void Delete(string id);
}

public class GenericService<T>(IGenericRepository<T> repository) : IGenericService<T> where T : BaseEntity
{
    private readonly IGenericRepository<T> _repository = repository;

    public ICollection<T>? GetAll(Expression<Func<T, object>>[] includes = null)
    {
        return _repository.GetAll(includes);
    }

    public T? GetById(string id, Expression<Func<T, object>>[] includes = null)
    {
        return _repository.GetById(id, includes);
    }

    public void Create(T entity)
    {
        _repository.Add(entity);
    }

    public void Update(T entity)
    {
        _repository.Update(entity);
    }

    public void Delete(string id)
    {
        T entity = _repository.GetById(id, null);

        if (entity != null)
        {
            _repository.Delete(entity);
        }
    }
}
