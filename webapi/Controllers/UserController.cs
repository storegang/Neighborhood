using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController(IBaseService<User> userService, IBaseService<Neighborhood> neighborhoodService, IUserSortService userSortService) : ControllerBase
{
    private readonly IBaseService<User> _userService = userService;
    private readonly IBaseService<Neighborhood> _neighborhoodService = neighborhoodService;
    private readonly IUserSortService _userSortService = userSortService;

    // GET: api/<UserController>
    [HttpGet]
    public async Task<ActionResult<UserCollectionDTO>> GetAll()
    {
        ICollection<User> users = await _userService.GetAll();
        
        UserCollectionDTO userDataCollection = new UserCollectionDTO(users);
        return Ok(userDataCollection);
    }

    // GET api/<UserController>/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetById(string id)
    {
        User? user = await _userService.GetById(id, null);
        
        if (user == null)
        {
            return NotFound();
        }

        var userData = new UserDTO(user);
        return Ok(userData);
    }

    // GET api/<UserController>/{id}
    [HttpGet("FromNeighborhood={id}")]
    public async Task<ActionResult<UserCollectionDTO>> GetAllUsersOfNeighborhoodId(string id)
    {
        Neighborhood? neighborhood = await _neighborhoodService.GetById(id, [c => c.Users]);

        if (neighborhood == null || neighborhood.Users == null)
        {
            return NotFound();
        }
        ICollection<User> users = neighborhood.Users;

        UserCollectionDTO userDataCollection = new UserCollectionDTO(users);

        return Ok(userDataCollection);
    }

    // GET api/<UserController>/{id}
    [HttpGet("FromNeighborhood={id}&role={role}")]
    public async Task<ActionResult<UserCollectionDTO>> GetAllUsersOfNeighborhoodIdSortedByRole(string id, string role)
    {
        if (!int.TryParse(role, out _))
        {
            return BadRequest();
        }
        UserSortService.Role sortByRole = (UserSortService.Role)int.Parse(role);

        Neighborhood? neighborhood = await _neighborhoodService.GetById(id, [c => c.Users]);

        if (neighborhood == null || neighborhood.Users == null)
        {
            return NotFound();
        }
        ICollection<User> users = neighborhood.Users;

        UserCollectionDTO userDataCollection = new UserCollectionDTO(_userSortService.GetUsersFromRole(users, sortByRole));

        return Ok(userDataCollection);
    }

    // GET api/<UserController>/{id}
    [HttpGet("FromNeighborhood={id}&sort={role}")]
    public async Task<ActionResult<UserCollectionDTO>> GetAllUsersOfNeighborhoodIdSortedByGroup(string id, string role)
    {
        if (!int.TryParse(role, out _))
        {
            return BadRequest();
        }
        UserSortService.RoleGroup sortByRole = (UserSortService.RoleGroup)int.Parse(role);

        Neighborhood? neighborhood = await _neighborhoodService.GetById(id, [c => c.Users]);

        if (neighborhood == null || neighborhood.Users == null)
        {
            return NotFound();
        }
        ICollection<User> users = neighborhood.Users;

        UserCollectionDTO userDataCollection = new UserCollectionDTO(_userSortService.GetUsersFromSort(users, sortByRole));

        return Ok(userDataCollection);
    }

    // POST api/<UserController>
    [HttpPost]
    public async Task<ActionResult<UserDTO>> Create(UserDTO userData)
    {
        // DEBUG: In this statement we make it so we can make fake users with custom Ids. Remove this in production.
        if (string.IsNullOrEmpty(userData.Id))
        {
            // Get the user_id from the token
            try
            {
                userData.Id = User.Claims.First(c => c.Type.Equals("user_id")).Value;
            }
            catch
            {
                return BadRequest();
            }
        }

        User user = new User(
            userData.Id,
            userData.Name,
            userData.Avatar,
            userData.NeighborhoodId);
        await _userService.Create(user);

        return CreatedAtAction(nameof(GetById), new { id = userData.Id }, userData);
    }

    // PUT api/<UserController>/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UserDTO userData)
    {
        User? existingUser = await _userService.GetById(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        userData.Id = id;
        User user = new User
            (
            userData.Id,
            userData.Name,
            userData.Avatar,
            userData.NeighborhoodId
            );

        if (userData.NeighborhoodId != null && userData.NeighborhoodId != existingUser.NeighborhoodId && existingUser.NeighborhoodId != null)
        {
            // Joining a neighborhood from another neighborhood

            Neighborhood? neighborhood = await _neighborhoodService.GetById(userData.NeighborhoodId, [c => c.Users]);
            Neighborhood? existingNeighborhood = await _neighborhoodService.GetById(existingUser.NeighborhoodId, [c => c.Users]);
            if (neighborhood == null)
            {
                return NotFound();
            }
            existingNeighborhood?.Users.Remove(user);
            await _neighborhoodService.Update(existingNeighborhood);

            neighborhood.Users.Add(user);
            await _neighborhoodService.Update(neighborhood);
        }
        else if (userData.NeighborhoodId == null && userData.NeighborhoodId != existingUser.NeighborhoodId)
        {
            // Leaving a neighborhood

            Neighborhood? neighborhood = await _neighborhoodService.GetById(userData.NeighborhoodId, [c => c.Users]);

            if (neighborhood != null)
            {
                neighborhood.Users.Remove(user);
                await _neighborhoodService.Update(neighborhood);
            }
        }
        else if (userData.NeighborhoodId != null && existingUser.NeighborhoodId == null)
        {
            // Joining a neighborhood without another neighborhood

            Neighborhood? neighborhood = await _neighborhoodService.GetById(userData.NeighborhoodId, [c => c.Users]);

            if (neighborhood == null)
            {
                return NotFound();
            }

            neighborhood.Users.Add(user);
            await _neighborhoodService.Update(neighborhood);
        }

        await _userService.Update(user);

        return NoContent();
    }

    // DELETE api/<UserController>/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        User? existingUser = await _userService.GetById(id, null);
        if (existingUser == null)
        {
            return NotFound();
        }

        await _userService.Delete(id);

        return NoContent();
    }
}
