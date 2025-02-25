using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;
using webapi.Repositories;
using Microsoft.Extensions.Hosting;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PostController(IGenericService<Post> postService, IGenericService<Category> categoryService, IGenericService<User> userService, LikeService likeService) : ControllerBase
{
    private readonly IGenericService<Post> _postService = postService;
    private readonly IGenericService<Category> _categoryService = categoryService;
    private readonly IGenericService<User> _userService = userService;
    private readonly LikeService _likeService = likeService;

    // GET: api/<PostController>
    [HttpGet]
    public ActionResult<PostCollectionDTO> GetAll()
    {
        ICollection<Post> postCollection = _postService.GetAll([c => c.User]);

        if (postCollection == null)
        {
            return NotFound();
        }

        PostCollectionDTO postDataCollection = new PostCollectionDTO(postCollection);

        Post[] posts = new Post[postCollection.Count()];
        posts = postCollection.ToArray();

        PostDTO[] postDTOs = new PostDTO[postDataCollection.Posts.Count()];
        postDTOs = postDataCollection.Posts.ToArray();

        for (int i = 0; i < postCollection.Count; i++)
        {
            if (posts[i].Id == postDTOs[i].Id)
            {
                postDTOs[i].LikedByCurrentUser = _likeService.LikePost(posts[i], this);
            }
        }

        postDataCollection.Posts = postDTOs;

        return Ok(postDataCollection);
    }

    [HttpGet("{id}")]
    public ActionResult<PostDTO> GetById(string id)
    {
        var post = _postService.GetById(id, [ c => c.User]);

        if (post == null)
        {
            return NotFound();
        }

        PostDTO postData = new PostDTO(post);

        postData.LikedByCurrentUser = _likeService.LikePost(post, this);

        return Ok(postData);
    }

    // GET api/<PostController>/{id}
    [HttpGet("FromCategory={id}")]
    public ActionResult<PostCollectionDTO> GetPostByCategoryId(string categoryId)
    {
        var category = _categoryService.GetById(categoryId, [c => c.Posts]);
        var postCollection = category.Posts;

        if (category == null || category.Posts == null)
        {
            return NotFound();
        }

        PostCollectionDTO postDataCollection = new PostCollectionDTO(postCollection);

        Post[] posts = new Post[postCollection.Count()];
        posts = postCollection.ToArray();

        PostDTO[] postDTOs = new PostDTO[postDataCollection.Posts.Count()];
        postDTOs = postDataCollection.Posts.ToArray();

        for (int i = 0; i < postCollection.Count; i++)
        {
            if (posts[i].Id == postDTOs[i].Id)
            {
                postDTOs[i].LikedByCurrentUser = _likeService.LikePost(posts[i], this);
            }
        }

        postDataCollection.Posts = postDTOs;

        return Ok(postDataCollection);
    }

    // GET api/<PostController>/FromCategory={postId}&Page={page}&Size={size}
    // GET api/<PostController>/FromCategory={postId}&Page={page}
    [HttpGet("FromCategory={postId}&Page={page}")]
    public ActionResult<PostCollectionDTO> GetSomeCommentsByPostId(string categoryId, string page, string size = "5")
    {
        var category = _categoryService.GetById(categoryId, [c => c.Posts]);


        if (category == null || category.Posts == null)
        {
            return NotFound();
        }

        if (!int.TryParse(page, out _) || !int.TryParse(size, out _))
        {
            return BadRequest();
        }

        var postCollection = category.Posts
        .Skip((int.Parse(page) - 1) * int.Parse(size))
        .Take(int.Parse(size))
        .ToList();;

        PostCollectionDTO postDataCollection = new PostCollectionDTO(postCollection);

        Post[] posts = new Post[postCollection.Count()];
        posts = postCollection.ToArray();

        PostDTO[] postDTOs = new PostDTO[postDataCollection.Posts.Count()];
        postDTOs = postDataCollection.Posts.ToArray();

        for (int i = 0; i < postCollection.Count; i++)
        {
            if (posts[i].Id == postDTOs[i].Id)
            {
                postDTOs[i].LikedByCurrentUser = _likeService.LikePost(posts[i], this);
            }
        }

        postDataCollection.Posts = postDTOs;

        return Ok(postDataCollection);
    }

    // POST api/<PostController>
    [HttpPost]
    public ActionResult<PostDTO> Create(PostDTO postData)
    {
        var category = _categoryService.GetById(postData.CategoryId, [c => c.Posts]);

        if (category == null)
        {
            return NotFound();
        }

        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_postService.GetById(newGuid) != null);

        postData.Id = newGuid;
        
        var post = new Post
        {
            Id = postData.Id,
            Title = postData.Title,
            Description = postData.Description,
            DatePosted = DateTime.Now,
            User = _userService.GetById(postData.AuthorUserId),
            CategoryId = postData.CategoryId,
            Category = category,
            Images = (ICollection<string>)postData.ImageUrls
        };

        _postService.Create(post);

        category.Posts.Add(post);
        _categoryService.Update(category);

        return CreatedAtAction(nameof(GetById), new { id = postData.Id }, postData);
    }

    // PUT api/<PostController>/{id}
    [HttpPut("{id}")]
    public IActionResult Update(string id, PostDTO postData)
    {
        var existingPost = _postService.GetById(id, [c => c.User]);
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
        _postService.Update(post);

        return NoContent();
    }

    // PUT api/<PostController>/Like/{postId}
    [HttpPut("Like/{postId}")]
    public IActionResult Like(string postId)
    {
        var post = _postService.GetById(postId, [c => c.User]);
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
        _postService.Update(post);
        return NoContent();
    }


    // DELETE api/<PostController>/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existingPost = _postService.GetById(id);
        if (existingPost == null)
        {
            return NotFound();
        }

        _postService.Delete(id);

        return NoContent();
    }
}
