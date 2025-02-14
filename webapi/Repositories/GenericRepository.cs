using webapi.Models;

namespace webapi.Repositories;

public interface IGenericRepository
{
    ICollection<Comment> GetAll();
    Comment GetById(string id);
    void Add(Comment comment);
    void Update(Comment comment);
    void Delete(Comment comment);
}

public class GenericRepository : IGenericRepository
{
    public void Add(Comment comment)
    {
        throw new NotImplementedException();
    }

    public void Delete(Comment comment)
    {
        throw new NotImplementedException();
    }

    public ICollection<Comment> GetAll()
    {
        throw new NotImplementedException();
    }

    public Comment GetById(string id)
    {
        throw new NotImplementedException();
    }

    public void Update(Comment comment)
    {
        throw new NotImplementedException();
    }
}
