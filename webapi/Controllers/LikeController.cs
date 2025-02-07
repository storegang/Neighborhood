using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using AutoMapper;

namespace webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LikeController(LikeService likeService, IMapper mapper) : ControllerBase
{
    private readonly LikeService _likeService = likeService;
    private readonly IMapper _mapper = mapper;

    // GET: api/<LikeController>
    [HttpGet]
    public ActionResult<IEnumerable<LikeDTO>> GetAll()
    {
        var likes = _likeService.GetAllLikes();
        var likeViewModels = _mapper.Map<IEnumerable<Like>, IEnumerable<LikeDTO>>(likes);
        return Ok(likeViewModels);
    }

    // GET api/<LikeController>/5
    [HttpGet("{id}")]
    public ActionResult<LikeDTO> GetById(string id)
    {
        var like = _likeService.GetLikeById(id);
        
        if (like == null)
        {
            return NotFound();
        }

        var likeViewModel = _mapper.Map<Like,  LikeDTO>(like);
        return Ok(likeViewModel);
    }

    // POST api/<LikeController>
    [HttpPost]
    public ActionResult<LikeDTO> Create(LikeDTO likeViewModel)
    {
        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_likeService.GetLikeById(newGuid) != null);

        likeViewModel.Id = newGuid;
        var like = _mapper.Map<LikeDTO, Like>(likeViewModel);
        _likeService.CreateLike(like);

        return CreatedAtAction(nameof(GetById), new { id = likeViewModel.Id }, likeViewModel);
    }

    // PUT api/<LikeController>/5
    [HttpPut("{id}")]
    public IActionResult Update(string id, LikeDTO likeViewModel)
    {
        var existingLike = _likeService.GetLikeById(id);
        if (existingLike == null)
        {
            return NotFound();
        }

        likeViewModel.Id = id;
        var like = _mapper.Map<LikeDTO, Like>(likeViewModel);
        _likeService.UpdateLike(like);

        return NoContent();
    }

    // DELETE api/<LikeController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existingLike = _likeService.GetLikeById(id);
        if (existingLike == null)
        {
            return NotFound();
        }

        _likeService.DeleteLike(id);

        return NoContent();
    }
}
