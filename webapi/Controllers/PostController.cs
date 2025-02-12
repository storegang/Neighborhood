using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
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
    public ActionResult<PostCollectionDTO> GetAll()
    {
        ICollection<Post> posts = _postService.GetAllPosts();
        PostCollectionDTO postViewModels = new PostCollectionDTO(posts);
        return Ok(postViewModels);
    }

    // GET api/<PostController>/5
    [HttpGet("{id}")]
    public ActionResult<PostDTO> GetById(string id)
    {
        var post = _postService.GetPostById(id);

        if (post == null)
        {
            return NotFound();
        }

        PostDTO postViewModel = new PostDTO(post);
        return Ok(postViewModel);
    }

    // POST api/<PostController>
    [HttpPost]
    public ActionResult<PostDTO> Create(PostDTO postViewModel)
    {
        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_postService.GetPostById(newGuid) != null);

        postViewModel.Id = newGuid;
        var post = _mapper.Map<PostDTO, Post>(postViewModel);
        _postService.CreatePost(post);

        return CreatedAtAction(nameof(GetById), new { id = postViewModel.Id }, postViewModel);
    }

    // PUT api/<PostController>/5
    [HttpPut("{id}")]
    public IActionResult Update(string id, PostDTO postViewModel)
    {
        var existingPost = _postService.GetPostById(id);
        if (existingPost == null)
        {
            return NotFound();
        }

        postViewModel.Id = id;
        var post = _mapper.Map<PostDTO, Post>(postViewModel);
        _postService.UpdatePost(post);

        return NoContent();
    }

    // DELETE api/<PostController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
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
