using webapi.Models;

namespace webapi.DTOs;

public class NeighborhoodDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public NeighborhoodDTO(Neighborhood neighborhood)
    {
        Id = neighborhood.Id;
        Name = neighborhood.Name;
        Description = neighborhood.Description;
    }
}

// In what use case do you need this?
public class NeighborhoodCollectionDTO
{
    public IEnumerable<NeighborhoodDTO> Neighborhoods { get; set; }

    public NeighborhoodCollectionDTO(ICollection<Neighborhood> neighborhoods)
    {
        Neighborhoods = neighborhoods.Select(neighborhood => new NeighborhoodDTO(neighborhood));
    }
}
