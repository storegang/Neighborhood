using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.ViewModels;
using AutoMapper;

namespace webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(PostService postService, IMapper mapper) : ControllerBase
{
    private readonly PostService _postService = postService;
    private readonly IMapper _mapper = mapper;

    // GET: api/<PostController>
    [HttpGet]
    public ActionResult<IEnumerable<PostViewModel>> GetAll()
    {
        var posts = _postService.GetAllPosts();
        var postViewModels = _mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(posts);
        return Ok(postViewModels);
    }

    // GET api/<PostController>/5
    [HttpGet("{id}")]
    public ActionResult<PostViewModel> GetById(int id)
    {
        var post = _postService.GetPostById(id);

        if (post == null)
        {
            return NotFound();
        }

        var postViewModel = _mapper.Map<Post, PostViewModel>(post);
        return Ok(postViewModel);
    }

    // POST api/<PostController>
    [HttpPost]
    public ActionResult<PostViewModel> Create(PostViewModel postViewModel)
    {
        var post = _mapper.Map<PostViewModel, Post>(postViewModel);
        _postService.CreatePost(post);

        postViewModel.Id = post.Id;

        return CreatedAtAction(nameof(GetById), new { id = postViewModel.Id }, postViewModel);
    }

    // PUT api/<PostController>/5
    [HttpPut("{id}")]
    public IActionResult Update(int id, PostViewModel postViewModel)
    {
        if (id != int.Parse(postViewModel.Id))
        {
            return BadRequest();
        }

        var existingPost = _postService.GetPostById(id);
        if (existingPost == null)
        {
            return NotFound();
        }

        var post = _mapper.Map<PostViewModel, Post>(postViewModel);
        _postService.UpdatePost(post);

        return NoContent();
    }

    // DELETE api/<PostController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existingPost = _postService.GetPostById(id);
        if (existingPost == null)
        {
            return NotFound();
        }

        _postService.DeletePost(id);

        return NoContent();
    }
}
