using Microsoft.EntityFrameworkCore;
using webapi.DataContexts;
using webapi.Models;

namespace webapi.Repositories;

public interface ICommentRepository
{
    ICollection<Comment> GetAll();
    Comment GetById(string id);
    Comment GetByIdExplicit(string id);
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

    public Comment GetById(string id)
    {
        return _context.Comments.Find(id);
    }

    public Comment GetByIdExplicit(string id)
    {
        var comment = _context.Comments
            //.Include(c => c.Post)
            .First(c => c.Id == id);
        return comment;

        // TODO: Include whatever could be needed
    }

    public void Add(Comment comment)
    {
        _context.Comments.Add(comment);
        _context.SaveChanges();
    }

    public void Update(Comment comment)
    {
        var existingComment = _context.Comments.Find(comment.Id);
        if (existingComment != null)
        {
            _context.Entry(existingComment).State = EntityState.Detached;
        }

        _context.Comments.Update(comment);
        _context.SaveChanges();
    }

    public void Delete(Comment comment)
    {
        _context.Comments.Remove(comment);
        _context.SaveChanges();
    }
}
