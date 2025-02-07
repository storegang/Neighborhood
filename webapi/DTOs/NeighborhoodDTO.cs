using webapi.Models;

namespace webapi.DTOs;

public class NeighborhoodDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}

// In what use case do you need this?
public class NeighborhoodsDTO
{
    public IEnumerable<NeighborhoodDTO> Neighborhoods { get; set; }
}
