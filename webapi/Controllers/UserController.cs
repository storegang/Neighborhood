﻿using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.ViewModels;
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
    public ActionResult<IEnumerable<UserViewModel>> GetAll()
    {
        var users = _userService.GetAllUsers();
        var userViewModels = _mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(users);
        return Ok(userViewModels);
    }

    // GET api/<UserController>/5
    [HttpGet("{id}")]
    public ActionResult<UserViewModel> GetById(string id)
    {
        var user = _userService.GetUserById(id);
        
        if (user == null)
        {
            return NotFound();
        }

        var userViewModel = _mapper.Map<User,  UserViewModel>(user);
        return Ok(userViewModel);
    }

    // POST api/<UserController>
    [HttpPost]
    public ActionResult<UserViewModel> Create(UserViewModel userViewModel)
    {
        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_userService.GetUserById(newGuid) != null);

        userViewModel.Id = newGuid;
        var user = _mapper.Map<UserViewModel, User>(userViewModel);
        _userService.CreateUser(user);

        return CreatedAtAction(nameof(GetById), new { id = userViewModel.Id }, userViewModel);
    }

    // PUT api/<UserController>/5
    [HttpPut("{id}")]
    public IActionResult Update(string id, UserViewModel userViewModel)
    {
        var existingUser = _userService.GetUserById(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        userViewModel.Id = id;
        var user = _mapper.Map<UserViewModel, User>(userViewModel);
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
