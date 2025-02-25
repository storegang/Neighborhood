﻿using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class NeighborhoodController(INeighborhoodService neighborhoodService) : ControllerBase
{
    private readonly INeighborhoodService _neighborhoodService = neighborhoodService;

    // GET: api/<NeighborhoodController>
    [HttpGet]
    public ActionResult<NeighborhoodCollectionDTO> GetAll()
    {
        ICollection<Neighborhood> neighborhoods = _neighborhoodService.GetAllNeighborhoods();
        NeighborhoodCollectionDTO neighborhoodDataCollection = new NeighborhoodCollectionDTO(neighborhoods);
        return Ok(neighborhoodDataCollection);
    }

    // GET api/<NeighborhoodController>/{id}
    [HttpGet("{id}")]
    public ActionResult<NeighborhoodDTO> GetById(string id)
    {
        var neighborhood = _neighborhoodService.GetNeighborhoodById(id);
        
        if (neighborhood == null)
        {
            return NotFound();
        }

        NeighborhoodDTO neighborhoodData = new NeighborhoodDTO(neighborhood);
        return Ok(neighborhoodData);
    }

    // POST api/<NeighborhoodController>
    [HttpPost]
    public ActionResult<NeighborhoodDTO> Create(NeighborhoodDTO neighborhoodData)
    {
        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_neighborhoodService.GetNeighborhoodById(newGuid) != null);

        neighborhoodData.Id = newGuid;
        var neighborhood = new Neighborhood 
        { 
            Id = neighborhoodData.Id,
            Name = neighborhoodData.Name,
            Description = neighborhoodData.Description
        };
        _neighborhoodService.CreateNeighborhood(neighborhood);

        return CreatedAtAction(nameof(GetById), new { id = neighborhoodData.Id }, neighborhoodData);
    }

    // PUT api/<NeighborhoodController>/{id}
    [HttpPut("{id}")]
    public IActionResult Update(string id, NeighborhoodDTO neighborhoodData)
    {
        var existingNeighborhood = _neighborhoodService.GetNeighborhoodById(id);
        if (existingNeighborhood == null)
        {
            return NotFound();
        }

        neighborhoodData.Id = id;
        var neighborhood = new Neighborhood
        {
            Id = neighborhoodData.Id,
            Name = neighborhoodData.Name,
            Description = neighborhoodData.Description
        };
        _neighborhoodService.UpdateNeighborhood(neighborhood);

        return NoContent();
    }

    // DELETE api/<NeighborhoodController>/{id}
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
