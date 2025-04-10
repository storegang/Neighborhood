using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.EntityFrameworkCore;
using webapi.Identity;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IBaseService<Neighborhood> neighborhoodService) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IBaseService<Neighborhood> _neighborhoodService = neighborhoodService;

    // GET: api/<UserController>
    [HttpGet]
    public async Task<ActionResult<UserCollectionDTO>> GetAll()
    {
        ICollection<User> users = _userManager.Users.ToArray();
        UserCollectionDTO userDataCollection = new(users);
        List<UserDTO> userDTOs = new();

        foreach (var user in users)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            userDTOs.Add(new UserDTO(user, roles));
        }

        userDataCollection.Users = userDTOs;

        return Ok(userDataCollection);
    }

    // GET api/<UserController>/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetById(string id)
    {
        User? user = await _userManager.FindByIdAsync(id);
        
        if (user == null)
        {
            return NotFound();
        }

        IEnumerable<string> roles = await _userManager.GetRolesAsync(user);

        UserDTO userData = new(user);
        userData.Roles = roles;
        return Ok(userData);
    }

    // GET api/<UserController>/FromNeighborhood={id}
    [HttpGet("FromNeighborhood={id}")]
    public async Task<ActionResult<UserCollectionDTO>> GetAllUsersOfNeighborhoodId(string id)
    {
        Neighborhood? neighborhood = await _neighborhoodService.GetById(id, [query => query.Include(c => c.Users)]);

        if (neighborhood == null || neighborhood.Users == null)
        {
            return NotFound();
        }
        ICollection<User> users = neighborhood.Users;

        UserCollectionDTO userDataCollection = new(users);

        return Ok(userDataCollection);
    }

    // GET api/<UserController>/FromNeighborhood={id}
    // GET api/<UserController>/FromNeighborhood={id}&Page={page}&Size={size}
    [HttpGet("FromNeighborhood={id}&Page={page}&Size={size}")]
    public async Task<ActionResult<UserCollectionDTO>> GetSomeUsersOfNeighborhoodId(string id, string page = "0", string size = "8")
    {
        if (!int.TryParse(page, out _) || !int.TryParse(size, out _))
        {
            return BadRequest();
        }

        Neighborhood? neighborhood = await _neighborhoodService.GetById
            (id, [query => query.Include(c => c.Users
                .Skip(int.Parse(page) * int.Parse(size))
                .Take(int.Parse(size)))]
            );

        if (neighborhood == null || neighborhood.Users == null)
        {
            return NotFound();
        }
        ICollection<User> users = neighborhood.Users;

        UserCollectionDTO userDataCollection = new(users);

        return Ok(userDataCollection);
    }

    // POST api/<UserController>
    [HttpPost]
    public async Task<ActionResult<UserDTO>> Create(UserDTO userData)
    {
        if (string.IsNullOrEmpty(userData.Id))
        {
            userData.Id = User.Claims.First(c => c.Type.Equals("user_id"))?.Value ?? "";
        }

        User? existingUser = await _userManager.FindByIdAsync(userData.Id);
        if (existingUser != null)
        {
            return Conflict();
        }

        User newUser = new()
        {
            Id = userData.Id,
            UserName = userData.Id,
            Name = userData.Name,
            Avatar = userData.Avatar
        };

        await _userManager.CreateAsync(newUser);

        return CreatedAtAction(nameof(GetById), new { id = userData.Id }, userData);
    }

    // PUT api/<UserController>/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UserDTO userData)
    {
        User? existingUser = await _userManager.FindByIdAsync(userData.Id);
        if (existingUser == null)
        {
            return NotFound();
        }

        userData.Id = id;

        if (userData.NeighborhoodId != null && userData.NeighborhoodId != existingUser.NeighborhoodId && existingUser.NeighborhoodId != null)
        {
            // Joining a neighborhood from another neighborhood

            Neighborhood? neighborhood = await _neighborhoodService.GetById(userData.NeighborhoodId, [query => query.AsNoTracking().Include(c => c.Users)]);
            Neighborhood? existingNeighborhood = await _neighborhoodService.GetById(existingUser.NeighborhoodId, [query => query.AsNoTracking().Include(c => c.Users)]);
            if (neighborhood == null)
            {
                return NotFound();
            }
            existingNeighborhood?.Users.Remove(existingUser);
            await _neighborhoodService.Update(existingNeighborhood);

            neighborhood.Users.Add(existingUser);
            await _neighborhoodService.Update(neighborhood);
        }
        else if (userData.NeighborhoodId == null && userData.NeighborhoodId != existingUser.NeighborhoodId)
        {
            // Leaving a neighborhood

            Neighborhood? neighborhood = await _neighborhoodService.GetById(userData.NeighborhoodId, [query => query.AsNoTracking().Include(c => c.Users)]);

            if (neighborhood != null)
            {
                neighborhood.Users.Remove(existingUser);
                await _neighborhoodService.Update(neighborhood);
            }
        }
        else if (userData.NeighborhoodId != null && existingUser.NeighborhoodId == null)
        {
            // Joining a neighborhood without another neighborhood

            Neighborhood? neighborhood = await _neighborhoodService.GetById(userData.NeighborhoodId, [query => query.AsNoTracking().Include(c => c.Users)]);

            if (neighborhood == null)
            {
                return NotFound();
            }

            neighborhood.Users.Add(existingUser);
            await _neighborhoodService.Update(neighborhood);
        }


        existingUser.Name = userData.Name;
        existingUser.Avatar = userData.Avatar;
        existingUser.NeighborhoodId = userData.NeighborhoodId;

        await _userManager.UpdateAsync(existingUser);

        return NoContent();
    }

    // DELETE api/<UserController>/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        User? existingUser = await _userManager.FindByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        await _userManager.DeleteAsync(existingUser);

        return NoContent();
    }





    // Get api/<UserController>/{id}/GetRole
    [HttpGet("{id}/GetRole")]
    public async Task<IActionResult> GetRole(string id)
    {
        User? existingUser = await _userManager.FindByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        IList<string> userRoles = await _userManager.GetRolesAsync(existingUser);

        return Ok(userRoles);
    }

    // PUT api/<UserController>/{id}/AddRole/{role}
    [HttpPut("{id}/AddRole/{role}")]
    public async Task<IActionResult> AddRole(string id, string role)
    {
        User? existingUser = await _userManager.FindByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        bool roleExists = await _roleManager.RoleExistsAsync(role);
        if (!roleExists)
        {
            return NotFound();
        }

        await _userManager.AddToRoleAsync(existingUser, role);

        return NoContent();
    }

    // PUT api/<UserController>/{id}/RemoveRole/{role}
    [HttpPut("{id}/RemoveRole/{role}")]
    public async Task<IActionResult> RemoveRole(string id, string role)
    {
        User? existingUser = await _userManager.FindByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        bool roleExists = await _roleManager.RoleExistsAsync(role);
        if (!roleExists)
        {
            return NotFound();
        }

        await _userManager.RemoveFromRoleAsync(existingUser, role);

        return NoContent();
    }
}
