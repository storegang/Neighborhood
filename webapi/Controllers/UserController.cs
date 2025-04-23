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
    public async Task<ActionResult<ServerUserCollectionDTO>> GetAll()
    {
        ICollection<User> users = _userManager.Users.ToArray();
        ServerUserCollectionDTO userDataCollection = new(users);
        List<ServerUserDTO> userDTOs = new();

        foreach (var user in users)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            userDTOs.Add(new ServerUserDTO(user, roles));
        }

        userDataCollection.Users = userDTOs;

        return Ok(userDataCollection);
    }

    // GET api/<UserController>/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ServerUserDTO>> GetById(string id)
    {
        User? user = await _userManager.FindByIdAsync(id);
        
        if (user == null)
        {
            return NotFound("User not found.");
        }

        IEnumerable<string> roles = await _userManager.GetRolesAsync(user);

        ServerUserDTO userData = new(user);
        userData.Roles = roles;
        return Ok(userData);
    }

    // GET api/<UserController>/FromNeighborhood={id}
    [HttpGet("FromNeighborhood={id}")]
    public async Task<ActionResult<ServerUserCollectionDTO>> GetAllUsersOfNeighborhoodId(string id)
    {
        Neighborhood? neighborhood = await _neighborhoodService.GetById(id, [query => query.Include(c => c.Users)]);

        if (neighborhood == null || neighborhood.Users == null)
        {
            return NotFound("Neighborhood not found.");
        }
        ICollection<User> users = neighborhood.Users;

        ServerUserCollectionDTO userDataCollection = new(users);

        return Ok(userDataCollection);
    }

    // GET api/<UserController>/FromNeighborhood={id}
    // GET api/<UserController>/FromNeighborhood={id}&Page={page}&Size={size}
    [HttpGet("FromNeighborhood={id}&Page={page}&Size={size}")]
    public async Task<ActionResult<ServerUserCollectionDTO>> GetSomeUsersOfNeighborhoodId(string id, string page = "0", string size = "8")
    {
        if (!int.TryParse(page, out _) || !int.TryParse(size, out _))
        {
            return BadRequest("Can not parse page or size.");
        }

        Neighborhood? neighborhood = await _neighborhoodService.GetById
            (id, [query => query.Include(c => c.Users
                .Skip(int.Parse(page) * int.Parse(size))
                .Take(int.Parse(size)))]
            );

        if (neighborhood == null || neighborhood.Users == null)
        {
            return NotFound("Neighborhood not found.");
        }
        ICollection<User> users = neighborhood.Users;

        ServerUserCollectionDTO userDataCollection = new(users);

        return Ok(userDataCollection);
    }

    // POST api/<UserController>
    // POST api/<UserController>/{id}
    [HttpPost]
    [HttpPost("{id}")]
    public async Task<ActionResult<ClientUserDTO>> Create(ClientUserDTO userData, string id = "")
    {
        string claimsId = id;
        if (string.IsNullOrEmpty(claimsId))
        {
            claimsId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value ?? "";
        }

        User? existingUser = await _userManager.FindByIdAsync(claimsId);
        if (existingUser != null)
        {
            return Conflict("User already exists.");
        }

        User newUser = new()
        {
            Id = claimsId,
            UserName = claimsId,
            Name = userData.Name,
            Avatar = userData.Avatar
        };

        await _userManager.CreateAsync(newUser);

        return CreatedAtAction(nameof(GetById), new { id = claimsId }, userData);
    }

    // PUT api/<UserController>/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, ClientUserDTO userData)
    {
        User? existingUser = await _userManager.FindByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound("User not found.");
        }

        existingUser.Name = userData.Name;
        existingUser.Avatar = userData.Avatar;

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
            return NotFound("User not found.");
        }

        await _userManager.DeleteAsync(existingUser);

        return NoContent();
    }



    // TODO: Consider relocating some or most of the bellow functions.

    // PUT api/<UserController>/{userId}&SetNeighborhood={neighborhoodId}
    [HttpPut("{userId}&SetNeighborhood={neighborhoodId}")]
    public async Task<IActionResult> SetNeighborhood(string userId, string neighborhoodId)
    {
        User? existingUser = await _userManager.FindByIdAsync(userId);
        if (existingUser == null)
        {
            return NotFound("User not found.");
        }

        if (neighborhoodId == existingUser.NeighborhoodId)
        {
            return NoContent();
        }

        Neighborhood? newNeighborhood = await _neighborhoodService.GetById(neighborhoodId, [query => query.AsNoTracking().Include(c => c.Users)]);
        if (newNeighborhood == null)
        {
            return NotFound("Neighborhood not found.");
        }

        // Belongs to a neighborhood already and need to leave that one.
        if (!string.IsNullOrEmpty(existingUser.NeighborhoodId))
        {
            Neighborhood? previousNeighborhood = await _neighborhoodService.GetById(existingUser.NeighborhoodId, [query => query.AsNoTracking().Include(c => c.Users)]);
            
            if (previousNeighborhood != null)
            {
                // TODO: Check if this is the last board member in the neighborhood and don't allow them to leave if so.
                previousNeighborhood?.Users.Remove(existingUser);
                await _neighborhoodService.Update(previousNeighborhood);
            }
        }

        newNeighborhood.Users.Add(existingUser);
        await _neighborhoodService.Update(newNeighborhood);

        existingUser.NeighborhoodId = neighborhoodId;
        await _userManager.AddToRoleAsync(existingUser, UserRoles.Tenant);
        await _userManager.UpdateAsync(existingUser);

        return NoContent();
    }

    // PUT api/<UserController>/{userId}&LeaveNeighborhood
    [HttpPut("{userId}&LeaveNeighborhood")]
    public async Task<IActionResult> LeaveNeighborhood(string userId)
    {
        User? existingUser = await _userManager.FindByIdAsync(userId);
        if (existingUser == null)
        {
            return NotFound("User not found.");
        }

        Neighborhood? neighborhood = await _neighborhoodService.GetById(existingUser.NeighborhoodId, [query => query.AsNoTracking().Include(c => c.Users)]);

        if (neighborhood != null)
        {
            neighborhood.Users.Remove(existingUser);
            await _neighborhoodService.Update(neighborhood);
        }

        // TODO: Check if this is the last board member in the neighborhood and don't allow them to leave if so.

        existingUser.NeighborhoodId = null;
        await _userManager.RemoveFromRolesAsync(existingUser, [UserRoles.Tenant, UserRoles.BoardMember]);
        await _userManager.UpdateAsync(existingUser);

        return NoContent();
    }

    [Authorize(Roles = UserRoles.BoardMember)]
    // PUT api/<UserController>/{userId}&AssignAsBoardMember
    [HttpPut("{userId}&AssignAsBoardMember")]
    public async Task<IActionResult> AssignAsBoardMember(string userId)
    {
        User? existingUser = await _userManager.FindByIdAsync(userId);
        if (existingUser == null)
        {
            return NotFound("Target user not found.");
        }

        string claimsId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value ?? "";
        User? claimUser = await _userManager.FindByIdAsync(claimsId);
        if (claimUser == null)
        {
            return NotFound("Requesting user not found.");
        }

        if (existingUser.NeighborhoodId != claimUser.NeighborhoodId)
        {
            return BadRequest("User is not a member of this neighborhood.");
        }

        Neighborhood? neighborhood = await _neighborhoodService.GetById(existingUser.NeighborhoodId, [query => query.AsNoTracking().Include(c => c.Users)]);

        if (neighborhood == null)
        {
            return NotFound("Requesting user has no neighborhood.");
        }

        await _userManager.AddToRoleAsync(existingUser, UserRoles.BoardMember);
        await _userManager.UpdateAsync(existingUser);

        return NoContent();
    }

    [Authorize(Roles = UserRoles.BoardMember)]
    // PUT api/<UserController>/{userId}&UnassignAsBoardMember
    [HttpPut("{userId}&UnassignAsBoardMember")]
    public async Task<IActionResult> UnassignAsBoardMember(string userId)
    {
        string claimsId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value ?? "";
        if (userId == claimsId)
        {
            return Unauthorized("Users can not remove their own board member role.");
        }

        User? existingUser = await _userManager.FindByIdAsync(userId);
        if (existingUser == null)
        {
            return NotFound("Target user not found.");
        }

        User? claimUser = await _userManager.FindByIdAsync(claimsId);
        if (claimUser == null)
        {
            return NotFound("Requesting user not found.");
        }

        if (existingUser.NeighborhoodId != claimUser.NeighborhoodId)
        {
            return BadRequest("User is not a member of this neighborhood.");
        }

        Neighborhood? neighborhood = await _neighborhoodService.GetById(existingUser.NeighborhoodId, [query => query.AsNoTracking().Include(c => c.Users)]);

        if (neighborhood == null)
        {
            return NotFound("Requesting user has no neighborhood.");
        }

        await _userManager.RemoveFromRoleAsync(existingUser, UserRoles.BoardMember);
        await _userManager.UpdateAsync(existingUser);

        return NoContent();
    }





    // Get api/<UserController>/{id}&GetRole
    [HttpGet("{id}&GetRole")]
    public async Task<IActionResult> GetRole(string id)
    {
        User? existingUser = await _userManager.FindByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound("User not found.");
        }

        IList<string> userRoles = await _userManager.GetRolesAsync(existingUser);

        return Ok(userRoles);
    }

    // PUT api/<UserController>/{id}&AddRole={role}
    [HttpPut("{id}&AddRole={role}")]
    public async Task<IActionResult> AddRole(string id, string role)
    {
        User? existingUser = await _userManager.FindByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound("User not found.");
        }

        bool roleExists = await _roleManager.RoleExistsAsync(role);
        if (!roleExists)
        {
            return NotFound("Role not found.");
        }

        await _userManager.AddToRoleAsync(existingUser, role);

        return NoContent();
    }

    // PUT api/<UserController>/{id}&RemoveRole={role}
    [HttpPut("{id}&RemoveRole={role}")]
    public async Task<IActionResult> RemoveRole(string id, string role)
    {
        User? existingUser = await _userManager.FindByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound("User not found.");
        }

        bool roleExists = await _roleManager.RoleExistsAsync(role);
        if (!roleExists)
        {
            return NotFound("Role not found.");
        }

        await _userManager.RemoveFromRoleAsync(existingUser, role);

        return NoContent();
    }
}
