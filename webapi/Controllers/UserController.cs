using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController(UserService userService) : ControllerBase
{
    private readonly UserService _userService = userService;

    // GET: api/<UserController>
    [HttpGet]
    public ActionResult<UserCollectionDTO> GetAll()
    {
        ICollection<User> users = _userService.GetAllUsers();
        UserCollectionDTO userDataCollection = new UserCollectionDTO(users);
        return Ok(userDataCollection);
    }

    // GET api/<UserController>/{id}
    [HttpGet("{id}")]
    public ActionResult<UserDTO> GetById(string id)
    {
        var user = _userService.GetUserById(id);
        
        if (user == null)
        {
            return NotFound();
        }

        var userData = new UserDTO(user);
        return Ok(userData);
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

        var user = new User
        {
            Id = userData.Id,
            Name = userData.Name,
            Avatar = userData.Avatar
        };
        _userService.CreateUser(user);

        return CreatedAtAction(nameof(GetById), new { id = userData.Id }, userData);
    }

    // PUT api/<UserController>/{id}
    [HttpPut("{id}")]
    public IActionResult Update(string id, UserDTO userData)
    {
        var existingUser = _userService.GetUserById(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        userData.Id = id;
        var user = new User 
        {
            Id = userData.Id,
            Name = userData.Name,
            Avatar = userData.Avatar,
            NeighborhoodId = userData.NeighborhoodId
        };
        _userService.UpdateUser(user);

        return NoContent();
    }

    // DELETE api/<UserController>/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existingUser = _userService.GetUserById(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        _userService.DeleteUser(id);

        return NoContent();
    }
}
