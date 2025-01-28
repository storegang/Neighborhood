using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.ViewModels;
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
    public ActionResult<IEnumerable<LikeViewModel>> GetAll()
    {
        var likes = _likeService.GetAllLikes();
        var likeViewModels = _mapper.Map<IEnumerable<Like>, IEnumerable<LikeViewModel>>(likes);
        return Ok(likeViewModels);
    }

    // GET api/<LikeController>/5
    [HttpGet("{id}")]
    public ActionResult<LikeViewModel> GetById(int id)
    {
        var like = _likeService.GetLikeById(id);
        
        if (like == null)
        {
            return NotFound();
        }

        var likeViewModel = _mapper.Map<Like,  LikeViewModel>(like);
        return Ok(likeViewModel);
    }

    // POST api/<LikeController>
    [HttpPost]
    public ActionResult<LikeViewModel> Create(LikeViewModel likeViewModel)
    {
        var like = _mapper.Map<LikeViewModel, Like>(likeViewModel);
        _likeService.CreateLike(like);

        likeViewModel.Id = like.Id;

        return CreatedAtAction(nameof(GetById), new { id = likeViewModel.Id }, likeViewModel);
    }

    // PUT api/<LikeController>/5
    [HttpPut("{id}")]
    public IActionResult Update(int id, LikeViewModel likeViewModel)
    {
        if (id != int.Parse(likeViewModel.Id))
        {
            return BadRequest();
        }

        var existingLike = _likeService.GetLikeById(id);
        if (existingLike == null)
        {
            return NotFound();
        }

        var like = _mapper.Map<LikeViewModel, Like>(likeViewModel);
        _likeService.UpdateLike(like);

        return NoContent();
    }

    // DELETE api/<LikeController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
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
