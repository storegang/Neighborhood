using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;

namespace webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(PostService postService, UserService userService, CategoryService categoryService) : ControllerBase
{
    private readonly PostService _postService = postService;
    private readonly UserService _userService = userService;
    private readonly CategoryService _categoryService = categoryService;

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

    // GET api/<PostController>/5
    [HttpGet("FromCategory={id}")]
    public ActionResult<PostCollectionDTO> GetPostByCategoryId(string categoryId)
    {
        var category = _categoryService.GetCategoryByIdWithChildren(categoryId);
        var posts = category.Posts;

        if (category == null || category.Posts == null)
        {
            return NotFound();
        }



        PostCollectionDTO postViewModel = new PostCollectionDTO(posts);
        return Ok(postViewModel);
    }

    // GET api/<PostController>/FromCategory={postId}&Page={page}&Size={size}
    // GET api/<PostController>/FromCategory=5
    [HttpGet("FromCategory={postId}&Page={page}")]
    public ActionResult<PostCollectionDTO> GetSomeCommentsByPostId(string categoryId, string page, string size = "5")
    {
        var category = _categoryService.GetCategoryByIdWithChildren(categoryId);


        if (category == null || category.Posts == null)
        {
            return NotFound();
        }

        if (!int.TryParse(page, out _) || !int.TryParse(size, out _))
        {
            return BadRequest();
        }

        var posts = category.Posts
        .Skip((int.Parse(page) - 1) * int.Parse(size))
        .Take(int.Parse(size))
        .ToList();

        var postsViewModel = new PostCollectionDTO(posts);
        return Ok(postsViewModel);
    }

    // POST api/<PostController>
    [HttpPost]
    public ActionResult<PostDTO> Create(PostDTO postViewModel)
    {
        var category = _categoryService.GetCategoryById(postViewModel.CategoryId);

        if (category == null)
        {
            return NotFound();
        }

        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_postService.GetPostById(newGuid) != null);

        postViewModel.Id = newGuid;
        var post = new Post
        {
            Id = postViewModel.Id,
            Title = postViewModel.Title,
            Description = postViewModel.Description,
            DatePosted = DateTime.Now,
            User = _userService.GetUserById(postViewModel.AuthorUserId),
            CategoryId = postViewModel.CategoryId
        };
        _postService.CreatePost(post);

        category.Posts.Add(post);
        _categoryService.UpdateCategory(category);

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
        var post = new Post
        {
            Id = postViewModel.Id,
            Title = postViewModel.Title,
            Description = postViewModel.Description,
            DateLastEdited = DateTime.Now,
            User = _userService.GetUserById(postViewModel.AuthorUserId),
            LikedByUserID = existingPost.LikedByUserID
        };
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
