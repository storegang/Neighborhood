using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.ViewModels;
using webapi.Services;
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
        var postViewModels = _mapper.Map<IEnumerable<PostViewModel>>(posts);
        return Ok(postViewModels);
    }

    // GET /api/PostController/5?commentCount=10
    [HttpGet("{id}")]
    public ActionResult<PostViewModel> GetById(string id, [FromQuery] int commentCount = 5)
    {
        var post = _postService.GetPostById(id);

        if (post == null)
        {
            return NotFound();
        }

        var postViewModel = _mapper.Map<PostViewModel>(post);
        postViewModel.Comments = _mapper.Map<ICollection<CommentViewModel>>(post.Comments?.Take(commentCount));

        return Ok(postViewModel);
    }

    // POST api/<PostController>
    [HttpPost]
    public ActionResult<PostViewModel> Create(PostViewModel PostViewModel)
    {
        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_postService.GetPostById(newGuid) != null);

        PostViewModel.Id = newGuid;

        var post = _mapper.Map<Post>(PostViewModel);
        _postService.CreatePost(post);

        var postViewModel = _mapper.Map<PostViewModel>(post);
        return CreatedAtAction(nameof(GetById), new { id = postViewModel.Id }, postViewModel);
    }

    // PUT api/<PostController>/5
    [HttpPut("{id}")]
    public IActionResult Update(string id, PostViewModel postViewModel)
    {
        var existingPost = _postService.GetPostById(id);
        if (existingPost == null)
        {
            return NotFound();
        }

        postViewModel.Id = id;
        var post = _mapper.Map<PostViewModel, Post>(postViewModel);
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
