using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using AutoMapper;

namespace webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NeighborhoodController(NeighborhoodService neighborhoodService, IMapper mapper) : ControllerBase
{
    private readonly NeighborhoodService _neighborhoodService = neighborhoodService;
    private readonly IMapper _mapper = mapper;

    // GET: api/<NeighborhoodController>
    [HttpGet]
    public ActionResult<IEnumerable<NeighborhoodDTO>> GetAll()
    {
        var neighborhoods = _neighborhoodService.GetAllNeighborhoods();
        var neighborhoodViewModels = _mapper.Map<IEnumerable<Neighborhood>, IEnumerable<NeighborhoodDTO>>(neighborhoods);
        return Ok(neighborhoodViewModels);
    }

    // GET api/<NeighborhoodController>/5
    [HttpGet("{id}")]
    public ActionResult<NeighborhoodDTO> GetById(string id)
    {
        var neighborhood = _neighborhoodService.GetNeighborhoodById(id);
        
        if (neighborhood == null)
        {
            return NotFound();
        }

        var neighborhoodViewModel = _mapper.Map<Neighborhood,  NeighborhoodDTO>(neighborhood);
        return Ok(neighborhoodViewModel);
    }

    // POST api/<NeighborhoodController>
    [HttpPost]
    public ActionResult<NeighborhoodDTO> Create(NeighborhoodDTO neighborhoodViewModel)
    {
        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_neighborhoodService.GetNeighborhoodById(newGuid) != null);

        neighborhoodViewModel.Id = newGuid;
        var neighborhood = _mapper.Map<NeighborhoodDTO, Neighborhood>(neighborhoodViewModel);
        _neighborhoodService.CreateNeighborhood(neighborhood);

        return CreatedAtAction(nameof(GetById), new { id = neighborhoodViewModel.Id }, neighborhoodViewModel);
    }

    // PUT api/<NeighborhoodController>/5
    [HttpPut("{id}")]
    public IActionResult Update(string id, NeighborhoodDTO neighborhoodViewModel)
    {
        var existingNeighborhood = _neighborhoodService.GetNeighborhoodById(id);
        if (existingNeighborhood == null)
        {
            return NotFound();
        }

        neighborhoodViewModel.Id = id;
        var neighborhood = _mapper.Map<NeighborhoodDTO, Neighborhood>(neighborhoodViewModel);
        _neighborhoodService.UpdateNeighborhood(neighborhood);

        return NoContent();
    }

    // DELETE api/<NeighborhoodController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existingNeighborhood = _neighborhoodService.GetNeighborhoodById(id);
        if (existingNeighborhood == null)
        {
            return NotFound();
        }

        _neighborhoodService.DeleteNeighborhood(id);

        return NoContent();
    }
}
