﻿using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using webapi.Interfaces;
using Microsoft.EntityFrameworkCore;
using webapi.Identity;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PostController(IBaseService<Post> postService, IBaseService<Category> categoryService, ILikeService<Post> likeService, UserManager<User> userManager) : ControllerBase
{
    private readonly IBaseService<Post> _postService = postService;
    private readonly IBaseService<Category> _categoryService = categoryService;
    private readonly ILikeService<Post> _likeService = likeService;
    private readonly UserManager<User> _userManager = userManager;

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
                postDTOs[i].CommentCount = await _postService.Count(p => p.Id == postDTOs[i].Id, p => p.Comments.Count());
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
            return NotFound("Post not found.");
        }

        ServerPostDTO postData = new(post);

        postData.CommentCount = await _postService.Count(p => p.Id == postData.Id, p => p.Comments.Count());
        postData.LikedByCurrentUser = await _likeService.IsLiked(post.LikedByUserID, User.Claims.First(c => c.Type.Equals("user_id"))?.Value);

        return Ok(postData);
    }

    // GET api/<PostController>/{id}
    [HttpGet("FromCategory={id}")]
    public async Task<ActionResult<ServerPostCollectionDTO>> GetPostByCategoryId(string id)
    {
        Category? category = await _categoryService.GetById(id, [query => query.Include(c => c.Posts).ThenInclude(p => p.User)]);

        if (category == null || category.Posts == null)
        {
            return NotFound("Category not found.");
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
                postDTOs[i].CommentCount = await _postService.Count(p => p.Id == postDTOs[i].Id, p => p.Comments.Count());
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
            return BadRequest("Could not parse page or size.");
        }

        Category? category = await _categoryService.GetById(categoryId, [query => query.Include(c => c.Posts).ThenInclude(c => c.User)]);

        if (category == null || category.Posts == null)
        {
            return NotFound("Category not found.");
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
                postDTOs[i].CommentCount = await _postService.Count(p => p.Id == postDTOs[i].Id, p => p.Comments.Count());
                postDTOs[i].LikedByCurrentUser = await _likeService.IsLiked(posts[i].LikedByUserID, User.Claims.First(c => c.Type.Equals("user_id"))?.Value);
            }
        }

        postDataCollection.Posts = postDTOs;

        return Ok(postDataCollection);
    }

    // POST api/<PostController>
    [HttpPost]
    public async Task<ActionResult<ClientPostDTO>> Create(ClientPostDTO postData)
    {
        Category? category = await _categoryService.GetById(postData.CategoryId, [query => query.Include(c => c.Posts).ThenInclude(p => p.User)]);
        if (category == null)
        {
            return NotFound("Category not found.");
        }

        User? user = await _userManager.FindByIdAsync(User.Claims.First(c => c.Type.Equals("user_id")).Value);
        if (user == null)
        {
            return Unauthorized("User does not exist.");
        }

        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (await _postService.GetById(newGuid) != null);
        
        Post post = new()
        {
            Id = newGuid,
            Title = postData.Title,
            Description = postData.Description,
            DatePosted = DateTime.UtcNow,
            User = user,
            CategoryId = postData.CategoryId,
            Category = category,
            Images = (ICollection<string>?)postData.ImageUrls
        };

        await _postService.Create(post);

        category.Posts.Add(post);
        await _categoryService.Update(category);

        return CreatedAtAction(nameof(GetById), new { id = newGuid }, postData);
    }

    // PUT api/<PostController>/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, ClientPostDTO postData)
    {
        Post? existingPost = await _postService.GetById(id, [query => query.Include(c => c.User)]);
        if (existingPost == null)
        {
            return NotFound("Post not found.");
        }

        string claimsId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value ?? "";
        if (claimsId != existingPost.User.Id)
        {
            User? claimsUser = await _userManager.FindByIdAsync(claimsId);
            bool IsSameNeighborhood = claimsUser?.NeighborhoodId == existingPost.User.NeighborhoodId;
            bool IsBoardMember = await _userManager.IsInRoleAsync(claimsUser, UserRoles.BoardMember);

            if (!IsSameNeighborhood && !IsBoardMember)
            {
                return Unauthorized("User is not the owner of this post or a board member in this neighborhood.");
            }
        }

        Post post = new()
        {
            Id = existingPost.Id,
            Title = postData.Title,
            Description = postData.Description,
            DatePosted = existingPost.DatePosted,
            DateLastEdited = DateTime.UtcNow,
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
            return NotFound("Post not found.");
        }
        string? userId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value;

        if (userId == null)
        {
            return Unauthorized("User does not exist.");
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
            return NotFound("Post not found.");
        }

        string claimsId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value ?? "";
        if (claimsId != existingPost.User.Id)
        {
            User? claimsUser = await _userManager.FindByIdAsync(claimsId);
            bool IsSameNeighborhood = claimsUser?.NeighborhoodId == existingPost.User.NeighborhoodId;
            bool IsBoardMember = await _userManager.IsInRoleAsync(claimsUser, UserRoles.BoardMember);

            if (!IsSameNeighborhood && !IsBoardMember)
            {
                return Unauthorized("User is not the owner of this post or a board member in this neighborhood.");
            }
        }

        await _postService.Delete(id);

        return NoContent();
    }
}
