using webapi.Models;
using webapi.Services;

namespace webapi.Tests.Services;

public class NeighborhoodServiceFake : INeighborhoodService
{
    private readonly List<Neighborhood> _neighborhoodRepository;

    public NeighborhoodServiceFake()
    {
        _neighborhoodRepository = new List<Neighborhood>()
        {
            new Neighborhood()
            {
                Id = "1",
                Name = "Neighborhood 1",
                Description = "Et fint boretslag :)",
                Categories = [
                    new Category()
                    {
                        Id = "1",
                        Name = "Cat 1",
                        Color = "Content 1",
                        NeighborhoodId = "1"
                    },
                    new Category()
                    {
                        Id = "2",
                        Name = "Cat 2",
                        Color = "Content 2",
                        NeighborhoodId = "1"
                    },
                ]
            },
            new Neighborhood()
            {
                Id = "2",
                Name = "Neighborhood 1",
                Description = "Et fint boretslag :)",
                Categories = [
                    new Category()
                    {
                        Id = "1",
                        Name = "Cat 1",
                        Color = "Content 1",
                        NeighborhoodId = "2"
                    },
                    new Category()
                    {
                        Id = "2",
                        Name = "Cat 2",
                        Color = "Content 2",
                        NeighborhoodId = "2"
                    },
                ]
            },
            new Neighborhood()
            {
                Id = "3",
                Name = "Neighborhood 1",
                Description = "Et fint boretslag :)",
                Categories = [
                    new Category()
                    {
                        Id = "1",
                        Name = "Cat 1",
                        Color = "Content 1",
                        NeighborhoodId = "3"
                    },
                    new Category()
                    {
                        Id = "2",
                        Name = "Cat 2",
                        Color = "Content 2",
                        NeighborhoodId = "3"
                    },
                ]
            },
        };
    }

    public ICollection<Neighborhood> GetAllNeighborhoods()
    {
        return _neighborhoodRepository;
    }

    public Neighborhood GetNeighborhoodById(string id)
    {
        return _neighborhoodRepository.Where(c => c.Id == id).FirstOrDefault();
    }

    public Neighborhood GetNeighborhoodByIdWithChildren(string id)
    {
        return _neighborhoodRepository.Where(c => c.Id == id).FirstOrDefault();
    }

    public void CreateNeighborhood(Neighborhood neighborhood)
    {
        _neighborhoodRepository.Add(neighborhood);
    }

    public void UpdateNeighborhood(Neighborhood neighborhood)
    {
        Neighborhood oldNeighborhood = _neighborhoodRepository.Where(c => c.Id == neighborhood.Id).FirstOrDefault();

        if (oldNeighborhood != null)
        {
            _neighborhoodRepository.Remove(oldNeighborhood);
        }

        oldNeighborhood = neighborhood;
    }

    public void DeleteNeighborhood(string id)
    {
        Neighborhood neighborhood = _neighborhoodRepository.Where(c => c.Id == id).FirstOrDefault();

        if (neighborhood != null)
        {
            _neighborhoodRepository.Remove(neighborhood);
        }
    }
}
