using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using AutoMapper;

namespace webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(UserService userService, IMapper mapper) : ControllerBase
{
    private readonly UserService _userService = userService;
    private readonly IMapper _mapper = mapper;

    // GET: api/<UserController>
    [HttpGet]
    public ActionResult<UserCollectionDTO> GetAll()
    {
        ICollection<User> users = _userService.GetAllUsers();
        UserCollectionDTO userViewModels = new UserCollectionDTO(users);
        return Ok(userViewModels);
    }

    // GET api/<UserController>/5
    [HttpGet("{id}")]
    public ActionResult<UserDTO> GetById(string id)
    {
        var user = _userService.GetUserById(id);
        
        if (user == null)
        {
            return NotFound();
        }

        var userViewModel = new UserDTO(user);
        return Ok(userViewModel);
    }

    // POST api/<UserController>
    [HttpPost]
    public ActionResult<UserDTO> Create(UserDTO userViewModel)
    {
        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_userService.GetUserById(newGuid) != null);

        userViewModel.Id = newGuid;
        var user = new User
        {
            Id = userViewModel.Id,
            Name = userViewModel.Name,
            Avatar = userViewModel.Avatar
        };
        _userService.CreateUser(user);

        return CreatedAtAction(nameof(GetById), new { id = userViewModel.Id }, userViewModel);
    }

    // PUT api/<UserController>/5
    [HttpPut("{id}")]
    public IActionResult Update(string id, UserDTO userViewModel)
    {
        var existingUser = _userService.GetUserById(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        userViewModel.Id = id;
        var user = new User 
        {
            Id = userViewModel.Id,
            Name = userViewModel.Name,
            Avatar = userViewModel.Avatar
        };
        _userService.UpdateUser(user);

        return NoContent();
    }

    // DELETE api/<UserController>/5
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
