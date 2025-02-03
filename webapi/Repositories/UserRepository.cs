using Microsoft.EntityFrameworkCore;
using webapi.DataContexts;
using webapi.Models;

namespace webapi.Repositories;

public interface IUserRepository
{
    ICollection<User> GetAll();
    User GetById(string id);
    void Add(User user);
    void Update(User user);
    void Delete(User user);
}

public class UserRepository(NeighborhoodContext context) : IUserRepository
{
    private readonly NeighborhoodContext _context = context;

    public ICollection<User> GetAll()
    {
        return _context.Users.ToList();
    }

    public User GetById(string id)
    {
        return _context.Users.Find(id);
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(User user)
    {
        var existingUser = _context.Users.Find(user.Id);
        if (existingUser != null)
        {
            _context.Entry(existingUser).State = EntityState.Detached;
        }

        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
    }
}
