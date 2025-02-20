using Microsoft.EntityFrameworkCore;
using webapi.DataContexts;
using webapi.Models;

namespace webapi.Repositories;

public interface IChildrenRepository<T1, T2> where T1 : class where T2 : class
{
    T1 GetByIdWithChildren(string id);
}

public interface IGetUserRepository<T1> where T1 : class
{
    T1 GetByIdWithUser(string id);
}

public interface IGenericRepository<T1> where T1 : class
{
    ICollection<T1> GetAll();
    T1 GetById(string id);
    void Add(T1 entity);
    void Update(T1 entity);
    void Delete(T1 entity);
}

public class GenericRepository<T1> : IGenericRepository<T1> where T1 : class
{
    private readonly NeighborhoodContext _context;
    private readonly DbSet<T1> _dbSet;

    public GenericRepository(NeighborhoodContext context)
    {
        _context = context;
        _dbSet = _context.Set<T1>();
    }

    public ICollection<T1> GetAll()
    {
        return _dbSet.ToList();
    }

    public T1 GetById(string id)
    {
        return _dbSet.Find(id);
    }

    public void Add(T1 entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public void Update(T1 entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void Delete(T1 entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }
}
