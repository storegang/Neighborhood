using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface INeighborhoodService
{
    ICollection<Neighborhood> GetAllNeighborhoods();
    Neighborhood GetNeighborhoodById(string id);
    Neighborhood GetNeighborhoodByIdWithChildren(string id);
    void CreateNeighborhood(Neighborhood neighborhood);
    void UpdateNeighborhood(Neighborhood neighborhood);
    void DeleteNeighborhood(string id);
}

public class NeighborhoodService(INeighborhoodRepository neighborhoodRepository) : INeighborhoodService
{
    private readonly INeighborhoodRepository _neighborhoodRepository = neighborhoodRepository;

    public ICollection<Neighborhood> GetAllNeighborhoods()
    {
        return _neighborhoodRepository.GetAll();
    }

    public Neighborhood GetNeighborhoodById(string id)
    {
        return _neighborhoodRepository.GetById(id);
    }

    public Neighborhood GetNeighborhoodByIdWithChildren(string id)
    {
        return _neighborhoodRepository.GetByIdWithChildren(id);
    }

    public void CreateNeighborhood(Neighborhood neighborhood)
    {
        _neighborhoodRepository.Add(neighborhood);
    }

    public void UpdateNeighborhood(Neighborhood neighborhood)
    {
        _neighborhoodRepository.Update(neighborhood);
    }

    public void DeleteNeighborhood(string id)
    {
        Neighborhood neighborhood = _neighborhoodRepository.GetById(id);

        if (neighborhood != null)
        {
            _neighborhoodRepository.Delete(neighborhood);
        }
    }
}
