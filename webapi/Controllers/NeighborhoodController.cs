using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using webapi.Identity;
using Microsoft.EntityFrameworkCore;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class NeighborhoodController(INeighborhoodService neighborhoodService, UserManager<User> userManager) : ControllerBase
{
    private readonly INeighborhoodService _neighborhoodService = neighborhoodService;
    private readonly UserManager<User> _userManager = userManager;

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
            return NotFound("Neighborhood not found.");
        }

        NeighborhoodDTO neighborhoodData = new(neighborhood);
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
        Neighborhood neighborhood = new() 
        { 
            Id = neighborhoodData.Id,
            Name = neighborhoodData.Name,
            Description = neighborhoodData.Description
        };

        string claimsId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value ?? "";
        User? existingUser = await _userManager.FindByIdAsync(claimsId);
        if (existingUser == null)
        {
            return Unauthorized("User does not exist.");
        }

        if (!string.IsNullOrEmpty(existingUser.NeighborhoodId))
        {
            return BadRequest("Can not create a new neighborhood while being a member of another.");
        }

        neighborhood.Users.Add(existingUser);
        await _neighborhoodService.Create(neighborhood);

        existingUser.NeighborhoodId = neighborhood.Id;
        await _userManager.AddToRolesAsync(existingUser, [UserRoles.Tenant, UserRoles.BoardMember]);
        await _userManager.UpdateAsync(existingUser);

        return CreatedAtAction(nameof(GetById), new { id = neighborhoodData.Id }, neighborhoodData);
    }

    [Authorize(Roles = UserRoles.BoardMember)]
    // PUT api/<NeighborhoodController>/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, NeighborhoodDTO neighborhoodData)
    {
        Neighborhood? existingNeighborhood = await _neighborhoodService.GetById(id);
        if (existingNeighborhood == null)
        {
            return NotFound("Neighborhood not found.");
        }

        string claimsId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value ?? "";
        User? claimsUser = await _userManager.FindByIdAsync(claimsId);
        if (claimsUser == null || claimsUser.NeighborhoodId != existingNeighborhood.Id)
        {
            return Unauthorized("User is not a member of this neighborhood or does not exist.");
        }

        neighborhoodData.Id = id;
        Neighborhood neighborhood = new()
        {
            Id = neighborhoodData.Id,
            Name = neighborhoodData.Name,
            Description = neighborhoodData.Description
        };
        await _neighborhoodService.Update(neighborhood);

        return NoContent();
    }

    [Authorize(Roles = UserRoles.BoardMember)]
    // DELETE api/<NeighborhoodController>/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        Neighborhood? existingNeighborhood = await _neighborhoodService.GetById(id, [query => query.Include(c => c.Users)]);
        if (existingNeighborhood == null)
        {
            return NotFound("Neighborhood not found.");
        }

        string claimsId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value ?? "";
        User? claimsUser = await _userManager.FindByIdAsync(claimsId);
        if (claimsUser == null || claimsUser.NeighborhoodId != existingNeighborhood.Id)
        {
            return Unauthorized("User is not a member of this neighborhood or does not exist.");
        }

        foreach (User user in existingNeighborhood.Users)
        {
            await _userManager.RemoveFromRolesAsync(user, [UserRoles.Tenant, UserRoles.BoardMember]);
            await _userManager.UpdateAsync(user);
        }

        await _neighborhoodService.Delete(id);

        return NoContent();
    }
}
