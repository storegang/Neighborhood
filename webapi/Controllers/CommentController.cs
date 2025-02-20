using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CommentController(CommentService commentService, UserService userService, PostService postService) : ControllerBase
{
    private readonly CommentService _commentService = commentService;
    private readonly UserService _userService = userService;
    private readonly PostService _postService = postService;

    // GET: api/<CommentController>
    [HttpGet]
    public ActionResult<CommentCollectionDTO> GetAll()
    {
        var comments = _commentService.GetAllComments();
        var commentDataCollection = new CommentCollectionDTO(comments);
        return Ok(commentDataCollection);
    }

    // GET api/<CommentController>/{id}
    [HttpGet("{id}")]
    public ActionResult<CommentDTO> GetById(string id)
    {
        var comment = _commentService.GetCommentById(id);
        
        if (comment == null)
        {
            return NotFound();
        }

        var commentData = new CommentDTO(comment);
        return Ok(commentData);
    }

    // GET api/<CommentController>/AllFromPost={postId}
    [HttpGet("AllFromPost={postId}")]
    public ActionResult<CommentCollectionDTO> GetCommentsByPostId(string postId)
    {
        var post = _postService.GetPostByIdWithChildren(postId);
        var comments = post.Comments;

        if (comments == null)
        {
            return NotFound();
        }

        var commentsData = new CommentCollectionDTO(comments);
        return Ok(commentsData);
    }

    // GET api/<CommentController>/FromPost={postId}&Page={page}&Size={size}
    // GET api/<CommentController>/FromPost={postId}&Page={page}
    [HttpGet("FromPost={postId}&Page={page}")]
    public ActionResult<CommentCollectionDTO> GetSomeCommentsByPostId(string postId, string page, string size = "5")
    {
        var post = _postService.GetPostByIdWithChildren(postId);


        if (post == null || post.Comments == null)
        {
            return NotFound();
        }

        if (!int.TryParse(page, out _) || !int.TryParse(size, out _))
        {
            return BadRequest();
        }

        var comments = post.Comments
        .Skip((int.Parse(page) - 1) * int.Parse(size))
        .Take(int.Parse(size))
        .ToList();

        var commentsData = new CommentCollectionDTO(comments);
        return Ok(commentsData);
    }

    // TODO: Add OrderBy 


    // POST api/<CommentController>
    [HttpPost]
    public ActionResult<CommentDTO> Create(CommentDTO commentData)
    {
        var post = _postService.GetPostById(commentData.ParentPostId);

        if (post == null)
        {
            return NotFound();
        }

        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_commentService.GetCommentById(newGuid) != null);

        commentData.Id = newGuid;
        var comment = new Comment
        {
            Id = commentData.Id,
            Content = commentData.Content,
            DatePosted = DateTime.Now,
            User = _userService.GetUserById(commentData.AuthorUserId),
            ParentPostId = commentData.ParentPostId,
            ImageUrl = commentData.ImageUrl
        };
        _commentService.CreateComment(comment);

        post.Comments.Add(comment);
        _postService.UpdatePost(post);

        return CreatedAtAction(nameof(GetById), new { id = commentData.Id }, commentData);
    }

    // PUT api/<CommentController>/{id}
    [HttpPut("{id}")]
    public IActionResult Update(string id, CommentDTO commentData)
    {
        var existingComment = _commentService.GetCommentById(id);
        if (existingComment == null)
        {
            return NotFound();
        }

        commentData.Id = id;
        var comment = new Comment 
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
        _commentService.UpdateComment(comment);

        return NoContent();
    }

    // DELETE api/<CommentController>/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existingComment = _commentService.GetCommentById(id);
        if (existingComment == null)
        {
            return NotFound();
        }

        _commentService.DeleteComment(id);

        return NoContent();
    }
}
