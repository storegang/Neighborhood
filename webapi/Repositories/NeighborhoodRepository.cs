using Microsoft.EntityFrameworkCore;
using webapi.DataContexts;
using webapi.Models;

namespace webapi.Repositories;

public interface INeighborhoodRepository
{
    ICollection<Neighborhood> GetAll();
    Neighborhood GetById(int id);
    void Add(Neighborhood neighborhood);
    void Update(Neighborhood neighborhood);
    void Delete(Neighborhood neighborhood);
}

public class NeighborhoodRepository(NeighborhoodContext context) : INeighborhoodRepository
{
    private readonly NeighborhoodContext _context = context;

    public ICollection<Neighborhood> GetAll()
    {
        return _context.Neighborhoods.ToList();
    }

    public Neighborhood GetById(int id)
    {
        return _context.Neighborhoods.Find(id);
    }

    public void Add(Neighborhood neighborhood)
    {
        _context.Neighborhoods.Add(neighborhood);
        _context.SaveChanges();
    }

    public void Update(Neighborhood neighborhood)
    {
        _context.Neighborhoods.Update(neighborhood);
        _context.SaveChanges();
    }

    public void Delete(Neighborhood neighborhood)
    {
        _context.Neighborhoods.Remove(neighborhood);
        _context.SaveChanges();
    }
}
