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
    public ActionResult<UserCollectionDTO> GetAll()
    {
        ICollection<User> users = _userService.GetAll();
        if (users == null)
        {
            Console.WriteLine("No users found. Have you forgotten to add any?");
            return NotFound();
        }
        UserCollectionDTO userDataCollection = new UserCollectionDTO(users);
        return Ok(userDataCollection);
    }

    // GET api/<UserController>/{id}
    [HttpGet("{id}")]
    public ActionResult<UserDTO> GetById(string id)
    {
        var user = _userService.GetById(id);
        
        if (user == null)
        {
            return NotFound();
        }

        var userData = new UserDTO(user);
        return Ok(userData);
    }

    // GET api/<UserController>/{id}
    [HttpGet("FromNeighborhood={id}")]
    public ActionResult<UserCollectionDTO> GetAllUsersOfNeighborhoodId(string id)
    {
        Neighborhood neighborhood = _neighborhoodService.GetById(id, [c => c.Users]);

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
    public ActionResult<UserCollectionDTO> GetAllUsersOfNeighborhoodIdSortedByRole(string id, string role)
    {
        if (!int.TryParse(role, out _))
        {
            return BadRequest();
        }
        UserSortService.Role sortByRole = (UserSortService.Role)int.Parse(role);

        Neighborhood neighborhood = _neighborhoodService.GetById(id, [c => c.Users]);

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
    public ActionResult<UserCollectionDTO> GetAllUsersOfNeighborhoodIdSortedBySort(string id, string role)
    {
        if (!int.TryParse(role, out _))
        {
            return BadRequest();
        }
        UserSortService.RoleSort sortByRole = (UserSortService.RoleSort)int.Parse(role);

        Neighborhood neighborhood = _neighborhoodService.GetById(id, [c => c.Users]);

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
    public ActionResult<UserDTO> Create(UserDTO userData)
    {
        // DEBUG: In this statement we make it so we can make fake users with custom Ids. Remove this in production.
        if (string.IsNullOrEmpty(userData.Id))
        {
            // Get the user_id from the token
            userData.Id = User.Claims.First(c => c.Type.Equals("user_id"))?.Value;
        }

        var user = new User(
            userData.Id,
            userData.Name,
            userData.Avatar,
            userData.NeighborhoodId);
        _userService.Create(user);

        return CreatedAtAction(nameof(GetById), new { id = userData.Id }, userData);
    }

    // PUT api/<UserController>/{id}
    [HttpPut("{id}")]
    public IActionResult Update(string id, UserDTO userData)
    {
        var existingUser = _userService.GetById(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        userData.Id = id;
        var user = new User
            (
            userData.Id,
            userData.Name,
            userData.Avatar,
            userData.NeighborhoodId
            );

        if (userData.NeighborhoodId != null && userData.NeighborhoodId != existingUser.NeighborhoodId && existingUser.NeighborhoodId != null)
        {
            // Joining a neighborhood from another neighborhood

            Neighborhood neighborhood = _neighborhoodService.GetById(userData.NeighborhoodId, [c => c.Users]);
            Neighborhood existingNeighborhood = _neighborhoodService.GetById(existingUser.NeighborhoodId, [c => c.Users]);
            if (neighborhood == null)
            {
                return NotFound();
            }
            existingNeighborhood.Users.Remove(user);
            _neighborhoodService.Update(existingNeighborhood);

            neighborhood.Users.Add(user);
            _neighborhoodService.Update(neighborhood);
        }
        else if (userData.NeighborhoodId == null && userData.NeighborhoodId != existingUser.NeighborhoodId)
        {
            // Leaving a neighborhood

            Neighborhood neighborhood = _neighborhoodService.GetById(userData.NeighborhoodId, [c => c.Users]);

            if (neighborhood != null)
            {
                neighborhood.Users.Remove(user);
                _neighborhoodService.Update(neighborhood);
            }
        }
        else if (userData.NeighborhoodId != null && existingUser.NeighborhoodId == null)
        {
            // Joining a neighborhood without another neighborhood

            Neighborhood neighborhood = _neighborhoodService.GetById(userData.NeighborhoodId, [c => c.Users]);

            if (neighborhood == null)
            {
                return NotFound();
            }

            neighborhood.Users.Add(user);
            _neighborhoodService.Update(neighborhood);
        }

        _userService.Update(user);

        return NoContent();
    }

    // DELETE api/<UserController>/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existingUser = _userService.GetById(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        _userService.Delete(id);

        return NoContent();
    }
}
