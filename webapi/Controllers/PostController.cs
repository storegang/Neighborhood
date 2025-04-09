using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;
using webapi.Repositories;
using Microsoft.Extensions.Hosting;
using webapi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PostController(IBaseService<Post> postService, IBaseService<Category> categoryService, IBaseService<User> userService, ILikeService<Post> likeService) : ControllerBase
{
    private readonly IBaseService<Post> _postService = postService;
    private readonly IBaseService<Category> _categoryService = categoryService;
    private readonly IBaseService<User> _userService = userService;
    private readonly ILikeService<Post> _likeService = likeService;

    // GET: api/<PostController>
    [HttpGet]
    public async Task<ActionResult<ServerPostCollectionDTO>> GetAll()
    {
        ICollection<Post> postCollection = await _postService.GetAll([query => query.Include(c => c.User)]);

        ServerPostCollectionDTO postDataCollection = new(postCollection);

        Post[] posts = new Post[postCollection.Count()];
        posts = postCollection.ToArray();

        ServerPostDTO[] postDTOs = new ServerPostDTO[postDataCollection.Posts.Count()];
        postDTOs = postDataCollection.Posts.ToArray();

        for (int i = 0; i < postCollection.Count; i++)
        {
            if (posts[i].Id == postDTOs[i].Id)
            {
                postDTOs[i].LikedByCurrentUser = await _likeService.IsLiked(posts[i].LikedByUserID, User.Claims.First(c => c.Type.Equals("user_id"))?.Value);
            }
        }

        postDataCollection.Posts = postDTOs;

        return Ok(postDataCollection);
    }

    // GET api/<PostController>/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ServerPostDTO>> GetById(string id)
    {
        Post? post = await _postService.GetById(id, [query => query.Include(c => c.User)]);

        if (post == null)
        {
            return NotFound();
        }

        ServerPostDTO postData = new(post);

        postData.LikedByCurrentUser = await _likeService.IsLiked(post.LikedByUserID, User.Claims.First(c => c.Type.Equals("user_id"))?.Value);

        return Ok(postData);
    }

    // GET api/<PostController>/{id}
    [HttpGet("FromCategory={id}")]
    public async Task<ActionResult<ServerPostCollectionDTO>> GetPostByCategoryId(string categoryId)
    {
        Category? category = await _categoryService.GetById(categoryId, [query => query.Include(c => c.Posts).ThenInclude(p => p.User)]);

        if (category == null || category.Posts == null)
        {
            return NotFound();
        }
        Post[] postCollection = category.Posts.ToArray();

        ServerPostCollectionDTO postDataCollection = new(postCollection);

        Post[] posts = new Post[postCollection.Count()];
        posts = postCollection.ToArray();

        ServerPostDTO[] postDTOs = new ServerPostDTO[postDataCollection.Posts.Count()];
        postDTOs = postDataCollection.Posts.ToArray();

        for (int i = 0; i < postCollection.Count(); i++)
        {
            if (posts[i].Id == postDTOs[i].Id)
            {
                postDTOs[i].LikedByCurrentUser = await _likeService.IsLiked(posts[i].LikedByUserID, User.Claims.First(c => c.Type.Equals("user_id"))?.Value);
            }
        }

        postDataCollection.Posts = postDTOs;

        return Ok(postDataCollection);
    }

    // GET api/<PostController>/FromCategory={postId}&Page={page}&Size={size}
    // GET api/<PostController>/FromCategory={postId}&Page={page}
    [HttpGet("FromCategory={postId}&Page={page}")]
    public async Task<ActionResult<ServerPostCollectionDTO>> GetSomeCommentsByPostId(string categoryId, string page = "0", string size = "5")
    {
        if (!int.TryParse(page, out _) || !int.TryParse(size, out _))
        {
            return BadRequest();
        }

        Category? category = await _categoryService.GetById(categoryId, [query => query.Include(c => c.Posts).ThenInclude(c => c.User)]);

        if (category == null || category.Posts == null)
        {
            return NotFound();
        }

        ICollection<Post> postCollection = category.Posts;

        ServerPostCollectionDTO postDataCollection = new(postCollection);

        Post[] posts = new Post[postCollection.Count()];
        posts = postCollection.ToArray();

        ServerPostDTO[] postDTOs = new ServerPostDTO[postDataCollection.Posts.Count()];
        postDTOs = postDataCollection.Posts.ToArray();

        for (int i = 0; i < postCollection.Count(); i++)
        {
            if (posts[i].Id == postDTOs[i].Id)
            {
                postDTOs[i].LikedByCurrentUser = await _likeService.IsLiked(posts[i].LikedByUserID, User.Claims.First(c => c.Type.Equals("user_id"))?.Value);
            }
        }

        postDataCollection.Posts = postDTOs;

        return Ok(postDataCollection);
    }

    // POST api/<PostController>
    [HttpPost]
    public async Task<ActionResult<ServerPostDTO>> Create(ClientPostDTO postData)
    {
        Category? category = await _categoryService.GetById(postData.CategoryId, [query => query.Include(c => c.Posts).ThenInclude(p => p.User)]);
        if (category == null)
        {
            return NotFound();
        }

        User? user = await _userService.GetById(User.Claims.First(c => c.Type.Equals("user_id")).Value);
        if (user == null)
        {
            return Unauthorized();
        }

        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (await _postService.GetById(newGuid) != null);

        postData.Id = newGuid;
        
        Post post = new()
        {
            Id = postData.Id,
            Title = postData.Title,
            Description = postData.Description,
            DatePosted = DateTime.Now,
            User = user,
            CategoryId = postData.CategoryId,
            Category = category,
            Images = (ICollection<string>?)postData.ImageUrls
        };

        await _postService.Create(post);

        category.Posts.Add(post);
        await _categoryService.Update(category);

        return CreatedAtAction(nameof(GetById), new { id = postData.Id }, postData);
    }

    // PUT api/<PostController>/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, ServerPostDTO postData)
    {
        Post? existingPost = await _postService.GetById(id, [query => query.Include(c => c.User)]);
        if (existingPost == null)
        {
            return NotFound();
        }

        postData.Id = id;
        Post post = new()
        {
            Id = existingPost.Id,
            Title = postData.Title,
            Description = postData.Description,
            DatePosted = existingPost.DatePosted,
            DateLastEdited = DateTime.Now,
            User = existingPost.User,
            CategoryId = existingPost.CategoryId,
            Category = existingPost.Category,
            Comments = existingPost.Comments,
            Images = (ICollection<string>?)postData.ImageUrls,
            LikedByUserID = existingPost.LikedByUserID
        };
        await _postService.Update(post);

        return NoContent();
    }

    // PUT api/<PostController>/Like/{postId}
    [HttpPut("Like/{postId}")]
    public async Task<IActionResult> Like(string postId)
    {
        Post? post = await _postService.GetById(postId, [query => query.Include(c => c.User)]);
        if (post == null)
        {
            return NotFound();
        }
        string? userId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

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
        await _postService.Update(post);
        return NoContent();
    }


    // DELETE api/<PostController>/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        Post? existingPost = await _postService.GetById(id);
        if (existingPost == null)
        {
            return NotFound();
        }

        await _postService.Delete(id);

        return NoContent();
    }
}
