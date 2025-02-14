using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public class NeighborhoodService(INeighborhoodRepository neighborhoodRepository)
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

    public Neighborhood GetNeighborhoodByIdExplicit(string id)
    {
        return _neighborhoodRepository.GetByIdExplicit(id);
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
