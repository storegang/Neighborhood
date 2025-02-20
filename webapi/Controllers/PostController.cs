using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace webapi.Controllers;

[Authorize]
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
        PostCollectionDTO postDataCollection = new PostCollectionDTO(posts);
        return Ok(postDataCollection);
    }

    // GET api/<PostController>/{id}
    [HttpGet("{id}")]
    public ActionResult<PostDTO> GetById(string id)
    {
        var post = _postService.GetPostById(id);

        if (post == null)
        {
            return NotFound();
        }

        PostDTO postData = new PostDTO(post);
        return Ok(postData);
    }

    // GET api/<PostController>/{id}
    [HttpGet("FromCategory={id}")]
    public ActionResult<PostCollectionDTO> GetPostByCategoryId(string categoryId)
    {
        var category = _categoryService.GetCategoryByIdWithChildren(categoryId);
        var posts = category.Posts;

        if (category == null || category.Posts == null)
        {
            return NotFound();
        }



        PostCollectionDTO postData = new PostCollectionDTO(posts);
        return Ok(postData);
    }

    // GET api/<PostController>/FromCategory={postId}&Page={page}&Size={size}
    // GET api/<PostController>/FromCategory={postId}&Page={page}
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

        var postsData = new PostCollectionDTO(posts);
        return Ok(postsData);
    }

    // POST api/<PostController>
    [HttpPost]
    public ActionResult<PostDTO> Create(PostDTO postData)
    {
        var category = _categoryService.GetCategoryById(postData.CategoryId);

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

        postData.Id = newGuid;
        var post = new Post
        {
            Id = postData.Id,
            Title = postData.Title,
            Description = postData.Description,
            DatePosted = DateTime.Now,
            User = _userService.GetUserById(postData.AuthorUserId),
            CategoryId = postData.CategoryId,
            Category = category,
            Images = (ICollection<string>)postData.ImageUrls
        };
        _postService.CreatePost(post);

        category.Posts.Add(post);
        _categoryService.UpdateCategory(category);

        return CreatedAtAction(nameof(GetById), new { id = postData.Id }, postData);
    }

    // PUT api/<PostController>/{id}
    [HttpPut("{id}")]
    public IActionResult Update(string id, PostDTO postData)
    {
        var existingPost = _postService.GetPostById(id);
        if (existingPost == null)
        {
            return NotFound();
        }

        postData.Id = id;
        var post = new Post
        {
            Id = postData.Id,
            Title = postData.Title,
            Description = postData.Description,
            DatePosted = existingPost.DatePosted,
            DateLastEdited = DateTime.Now,
            User = existingPost.User,
            CategoryId = existingPost.CategoryId,
            Category = existingPost.Category,
            Comments = existingPost.Comments,
            Images = (ICollection<string>)postData.ImageUrls,
            LikedByUserID = existingPost.LikedByUserID
        };
        _postService.UpdatePost(post);

        return NoContent();
    }

    [HttpPut("Like/{postId}")]
    public IActionResult Like(string postId)
    {
        var post = _postService.GetPostById(postId);
        if (post == null)
        {
            return NotFound();
        }
        var userId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value;

        if (post.LikedByUserID == null)
        {
            post.LikedByUserID = new List<string>();
        }
        if (post.LikedByUserID.Contains(userId))
        {
            post.LikedByUserID.Remove(userId);
        }
        else
        {
            post.LikedByUserID.Add(userId);
        }
        _postService.UpdatePost(post);
        return NoContent();
    }


    // DELETE api/<PostController>/{id}
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
