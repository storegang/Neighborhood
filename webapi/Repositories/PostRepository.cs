using Microsoft.EntityFrameworkCore;
using webapi.DataContexts;
using webapi.Models;

namespace webapi.Repositories;

public interface IPostRepository
{
    ICollection<Post> GetAll();
    Post GetById(int id);
    void Add(Post post);
    void Update(Post post);
    void Delete(Post post);
}

public class PostRepository(NeighborhoodContext context) : IPostRepository
{
    private readonly NeighborhoodContext _context = context;

    public ICollection<Post> GetAll()
    {
        return _context.Posts.ToList();
    }

    public Post GetById(int id)
    {
        return _context.Posts.Find(id);
    }

    public void Add(Post post)
    {
        _context.Posts.Add(post);
        _context.SaveChanges();
    }

    public void Update(Post post)
    {
        _context.Posts.Update(post);
        _context.SaveChanges();
    }

    public void Delete(Post post)
    {
        _context.Posts.Remove(post);
        _context.SaveChanges();
    }
}
