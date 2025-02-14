using Microsoft.EntityFrameworkCore;
using webapi.DataContexts;
using webapi.Models;

namespace webapi.Repositories;

public interface INeighborhoodRepository
{
    ICollection<Neighborhood> GetAll();
    Neighborhood GetById(string id);
    Neighborhood GetByIdExplicit(string id);
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

    public Neighborhood GetById(string id)
    {
        return _context.Neighborhoods.Find(id);
    }

    public Neighborhood GetByIdExplicit(string id)
    {
        var neighborhood = _context.Neighborhoods
            .Include(n => n.Categories)
            .First(n => n.Id == id);
        return neighborhood;
    }

    public void Add(Neighborhood neighborhood)
    {
        _context.Neighborhoods.Add(neighborhood);
        _context.SaveChanges();
    }

    public void Update(Neighborhood neighborhood)
    {
        var existingNeighborhood = _context.Neighborhoods.Find(neighborhood.Id);
        if (existingNeighborhood != null)
        {
            _context.Entry(existingNeighborhood).State = EntityState.Detached;
        }

        _context.Neighborhoods.Update(neighborhood);
        _context.SaveChanges();
    }

    public void Delete(Neighborhood neighborhood)
    {
        _context.Neighborhoods.Remove(neighborhood);
        _context.SaveChanges();
    }
}
