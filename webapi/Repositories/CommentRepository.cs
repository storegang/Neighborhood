using Microsoft.EntityFrameworkCore;
using webapi.DataContexts;
using webapi.Models;

namespace webapi.Repositories;

public interface ICommentRepository
{
    ICollection<Comment> GetAll();
    Comment GetById(int id);
    void Add(Comment comment);
    void Update(Comment comment);
    void Delete(Comment comment);
}

public class CommentRepository(NeighborhoodContext context) : ICommentRepository
{
    private readonly NeighborhoodContext _context = context;

    public ICollection<Comment> GetAll()
    {
        return _context.Comments.ToList();
    }

    public Comment GetById(int id)
    {
        return _context.Comments.Find(id);
    }

    public void Add(Comment comment)
    {
        _context.Comments.Add(comment);
        _context.SaveChanges();
    }

    public void Update(Comment comment)
    {
        _context.Comments.Update(comment);
        _context.SaveChanges();
    }

    public void Delete(Comment comment)
    {
        _context.Comments.Remove(comment);
        _context.SaveChanges();
    }
}
