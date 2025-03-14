using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class NeighborhoodController(INeighborhoodService neighborhoodService, IBaseService<User> userService) : ControllerBase
{
    private readonly INeighborhoodService _neighborhoodService = neighborhoodService;
    private readonly IBaseService<User> _userService = userService;

    // GET: api/<NeighborhoodController>
    [HttpGet]
    public async Task<ActionResult<NeighborhoodCollectionDTO>> GetAll()
    {
        ICollection<Neighborhood>? neighborhoods = await _neighborhoodService.GetAll();
        NeighborhoodCollectionDTO neighborhoodDataCollection = new NeighborhoodCollectionDTO(neighborhoods);
        return Ok(neighborhoodDataCollection);
    }

    // GET api/<NeighborhoodController>/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<NeighborhoodDTO>> GetById(string id)
    {
        Neighborhood? neighborhood = await _neighborhoodService.GetById(id);
        
        if (neighborhood == null)
        {
            return NotFound();
        }

        NeighborhoodDTO neighborhoodData = new NeighborhoodDTO(neighborhood);
        return Ok(neighborhoodData);
    }

    // POST api/<NeighborhoodController>
    [HttpPost]
    public async Task<ActionResult<NeighborhoodDTO>> Create(NeighborhoodDTO neighborhoodData)
    {
        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (await _neighborhoodService.GetById(newGuid) != null);

        neighborhoodData.Id = newGuid;
        Neighborhood neighborhood = new Neighborhood 
        { 
            Id = neighborhoodData.Id,
            Name = neighborhoodData.Name,
            Description = neighborhoodData.Description
        };
        await _neighborhoodService.Create(neighborhood);

        return CreatedAtAction(nameof(GetById), new { id = neighborhoodData.Id }, neighborhoodData);
    }

    // PUT api/<NeighborhoodController>/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, NeighborhoodDTO neighborhoodData)
    {
        Neighborhood? existingNeighborhood = await _neighborhoodService.GetById(id);
        if (existingNeighborhood == null)
        {
            return NotFound();
        }

        neighborhoodData.Id = id;
        Neighborhood neighborhood = new Neighborhood
        {
            Id = neighborhoodData.Id,
            Name = neighborhoodData.Name,
            Description = neighborhoodData.Description
        };
        await _neighborhoodService.Update(neighborhood);

        return NoContent();
    }

    // DELETE api/<NeighborhoodController>/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        Neighborhood? existingNeighborhood = await _neighborhoodService.GetById(id);
        if (existingNeighborhood == null)
        {
            return NotFound();
        }

        await _neighborhoodService.Delete(id);

        return NoContent();
    }
}
