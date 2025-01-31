using Microsoft.EntityFrameworkCore;
using webapi.DataContexts;
using webapi.Models;

namespace webapi.Repositories;

public interface ILikeRepository
{
    ICollection<Like> GetAll();
    Like GetById(string id);
    void Add(Like like);
    void Update(Like like);
    void Delete(Like like);
}

public class LikeRepository(NeighborhoodContext context) : ILikeRepository
{
    private readonly NeighborhoodContext _context = context;

    public ICollection<Like> GetAll()
    {
        return _context.Likes.ToList();
    }

    public Like GetById(string id)
    {
        return _context.Likes.Find(id);
    }

    public void Add(Like like)
    {
        _context.Likes.Add(like);
        _context.SaveChanges();
    }

    public void Update(Like like)
    {
        var existingLike = _context.Likes.Find(like.Id);
        if (existingLike != null)
        {
            _context.Entry(existingLike).State = EntityState.Detached;
        }

        _context.Likes.Update(like);
        _context.SaveChanges();
    }

    public void Delete(Like like)
    {
        _context.Likes.Remove(like);
        _context.SaveChanges();
    }
}
