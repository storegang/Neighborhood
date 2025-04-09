using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using webapi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CommentController(IBaseService<Comment> commentService, IBaseService<User> userService, IBaseService<Post> postService, ILikeService<Comment> likeService) : ControllerBase
{
    private readonly IBaseService<Comment> _commentService = commentService;
    private readonly IBaseService<User> _userService = userService;
    private readonly IBaseService<Post> _postService = postService;
    private readonly ILikeService<Comment> _likeService = likeService;

    // GET: api/<CommentController>
    [HttpGet]
    public async Task<ActionResult<CommentCollectionDTO>> GetAll()
    {
        ICollection<Comment> commentCollection = await _commentService.GetAll([query => query.Include(c => c.User)]);
        
        CommentCollectionDTO commentDataCollection = new(commentCollection);

        Comment[] comments = new Comment[commentCollection.Count()];
        comments = commentCollection.ToArray();

        CommentDTO[] commentDTOs = new CommentDTO[commentDataCollection.Comments.Count()];
        commentDTOs = commentDataCollection.Comments.ToArray();

        for (int i = 0; i < commentCollection.Count; i++)
        {
            if (comments[i].Id == commentDTOs[i].Id)
            {
                commentDTOs[i].LikedByCurrentUser = await _likeService.IsLiked(comments[i].LikedByUserID, User.Claims.First(c => c.Type.Equals("user_id"))?.Value);
            }
        }

        commentDataCollection.Comments = commentDTOs;


        return Ok(commentDataCollection);
    }

    // GET api/<CommentController>/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDTO>> GetById(string id)
    {
        Comment? comment = await _commentService.GetById(id);
        
        if (comment == null)
        {
            return NotFound();
        }

        CommentDTO commentData = new(comment);

        commentData.LikedByCurrentUser = await _likeService.IsLiked(comment.LikedByUserID, User.Claims.First(c => c.Type.Equals("user_id"))?.Value);

        return Ok(commentData);
    }

    // GET api/<CommentController>/AllFromPost={postId}
    [HttpGet("AllFromPost={postId}")]
    public async Task<ActionResult<CommentCollectionDTO>> GetCommentsByPostId(string postId)
    {
        Post? post = await _postService.GetById(postId, [query => query.Include(p => p.Comments).ThenInclude(c => c.User)]);

        if (post == null || post.Comments == null)
        {
            return NotFound();
        }
        ICollection<Comment>? commentCollection = post.Comments;

        CommentCollectionDTO commentDataCollection = new(commentCollection);

        ICollection<Comment> comments = new Comment[commentCollection.Count()];
        comments = commentCollection.ToArray();

        IEnumerable<CommentDTO> commentDTOs = new CommentDTO[commentDataCollection.Comments.Count()];
        commentDTOs = commentDataCollection.Comments.ToArray();


        foreach (CommentDTO commentData in commentDTOs)
        {
            Comment? comment = comments.FirstOrDefault(c => c.Id == commentData.Id);

            if (comment == null)
            {
                continue;
            }

            commentData.LikedByCurrentUser = await _likeService.IsLiked(comment.LikedByUserID, User.Claims.First(c => c.Type.Equals("user_id"))?.Value);
        }

        commentDataCollection.Comments = commentDTOs;

        return Ok(commentDataCollection);
    }

    // GET api/<CommentController>/FromPost={postId}&Page={page}&Size={size}
    // GET api/<CommentController>/FromPost={postId}&Page={page}
    [HttpGet("FromPost={postId}&Page={page}&Size={size}")]
    public async Task<ActionResult<CommentCollectionDTO>> GetSomeCommentsByPostId(string postId, string page, string size = "5")
    {
        if (!int.TryParse(page, out _) || !int.TryParse(size, out _))
        {
            return BadRequest();
        }

        Post? post = await _postService.GetById(postId, 
            [query => query.Include(p => p.Comments
                .Skip(int.Parse(page) * int.Parse(size))
                .Take(int.Parse(size)))
                .ThenInclude(c => c.User)]);

        if (post == null)
        {
            return NotFound();
        }

        ICollection<Comment> commentCollection = post.Comments;

        CommentCollectionDTO commentDataCollection = new(commentCollection);

        ICollection<Comment> comments = new Comment[commentCollection.Count()];
        comments = commentCollection.ToArray();

        IEnumerable<CommentDTO> commentDTOs = new CommentDTO[commentDataCollection.Comments.Count()];
        commentDTOs = commentDataCollection.Comments.ToArray();


        foreach (CommentDTO commentData in commentDTOs)
        {
            Comment? comment = comments.FirstOrDefault(c => c.Id == commentData.Id);

            if (comment == null)
            {
                continue;
            }

            commentData.LikedByCurrentUser = await _likeService.IsLiked(comment.LikedByUserID, User.Claims.First(c => c.Type.Equals("user_id"))?.Value);
        }

        commentDataCollection.Comments = commentDTOs;

        return Ok(commentDataCollection);
    }

    // TODO: Add OrderBy 


    // POST api/<CommentController>
    [HttpPost]
    public async Task<ActionResult<CommentDTO>> Create(CommentDTO commentData)
    {
        Post? post = await _postService.GetById(commentData.ParentPostId, [query => query.Include(c => c.User)]);

        if (post == null)
        {
            return NotFound();
        }

        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (await _commentService.GetById(newGuid) != null);

        commentData.Id = newGuid;
        Comment comment = new()
        {
            Id = commentData.Id,
            Content = commentData.Content,
            DatePosted = DateTime.Now,
            User = string.IsNullOrEmpty(commentData.AuthorUserId) ? await _userService.GetById(User.Claims.First(c => c.Type.Equals("user_id"))?.Value) : await _userService.GetById(commentData.AuthorUserId),
            ParentPostId = commentData.ParentPostId,
            ImageUrl = commentData.ImageUrl
        };
        await _commentService.Create(comment);

        post.Comments.Add(comment);
        await _postService.Update(post);

        return CreatedAtAction(nameof(GetById), new { id = commentData.Id }, commentData);
    }

    // PUT api/<CommentController>/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(CommentDTO commentData)
    {
        Comment? existingComment = await _commentService.GetById(commentData.Id, [query => query.Include(c => c.User)]);
        if (existingComment == null)
        {
            return NotFound();
        }

        Comment comment = new() 
        {
            Id = existingComment.Id,
            Content = commentData.Content,
            DatePosted = existingComment.DatePosted,
            DateLastEdited = DateTime.Now,
            User = existingComment.User,
            ParentPostId = existingComment.ParentPostId,
            ParentPost = existingComment.ParentPost,
            ImageUrl = commentData.ImageUrl,
            LikedByUserID = existingComment.LikedByUserID
        };
        await _commentService.Update(comment);

        return NoContent();
    }

    // PUT api/<CommentController>/Likes/{commentId}
    [HttpPut("Likes/{commentId}")]
    public async Task<IActionResult> Like(string commentId)
    {
        Comment? comment = await _commentService.GetById(commentId, [query => query.Include(c => c.User)]);
        if (comment == null)
        {
            return NotFound();
        }
        string? userId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        if (comment.LikedByUserID == null)
        {
            comment.LikedByUserID = new List<string>();
        }
        if (comment.LikedByUserID.Contains(userId))
        {
            comment.LikedByUserID.Remove(userId);
        }
        else
        {
            comment.LikedByUserID.Add(userId);
        }
        await _commentService.Update(comment);
        return NoContent();
    }

    // DELETE api/<CommentController>/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        Comment? existingComment = await _commentService.GetById(id);
        if (existingComment == null)
        {
            return NotFound();
        }

        await _commentService.Delete(id);

        return NoContent();
    }
}
