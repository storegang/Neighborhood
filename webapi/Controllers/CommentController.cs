using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;

namespace webapi.Controllers;

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
        var commentViewModels = new CommentCollectionDTO(comments);
        return Ok(commentViewModels);
    }

    // GET api/<CommentController>/5
    [HttpGet("{id}")]
    public ActionResult<CommentDTO> GetById(string id)
    {
        var comment = _commentService.GetCommentById(id);
        
        if (comment == null)
        {
            return NotFound();
        }

        var commentViewModel = new CommentDTO(comment);
        return Ok(commentViewModel);
    }

    // GET api/<CommentController>/FromPost=5
    [HttpGet("AllFromPost={postId}")]
    public ActionResult<CommentCollectionDTO> GetCommentsByPostId(string postId)
    {
        var post = _postService.GetPostByIdWithChildren(postId);
        var comments = post.Comments;

        if (comments == null)
        {
            return NotFound();
        }

        var commentsViewModel = new CommentCollectionDTO(comments);
        return Ok(commentsViewModel);
    }

    // GET api/<CommentController>/FromPost={postId}&Page={page}&Size={size}
    // GET api/<CommentController>/FromPost=5
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

        var commentsViewModel = new CommentCollectionDTO(comments);
        return Ok(commentsViewModel);
    }

    // TODO: Add OrderBy 


    // POST api/<CommentController>
    [HttpPost]
    public ActionResult<CommentDTO> Create(CommentDTO commentViewModel)
    {
        var post = _postService.GetPostById(commentViewModel.ParentPostId);

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

        commentViewModel.Id = newGuid;
        var comment = new Comment
        {
            Id = commentViewModel.Id,
            Content = commentViewModel.Content,
            DatePosted = DateTime.Now,
            User = _userService.GetUserById(commentViewModel.AuthorUserId),
            ParentPostId = commentViewModel.ParentPostId
        };
        _commentService.CreateComment(comment);

        post.Comments.Add(comment);
        _postService.UpdatePost(post);

        return CreatedAtAction(nameof(GetById), new { id = commentViewModel.Id }, commentViewModel);
    }

    // PUT api/<CommentController>/5
    [HttpPut("{id}")]
    public IActionResult Update(string id, CommentDTO commentViewModel)
    {
        var existingComment = _commentService.GetCommentById(id);
        if (existingComment == null)
        {
            return NotFound();
        }

        commentViewModel.Id = id;
        var comment = new Comment 
        {
            Id = commentViewModel.Id,
            Content = commentViewModel.Content,
            DateLastEdited = DateTime.Now,
            User = _userService.GetUserById(commentViewModel.AuthorUserId),
            ParentPostId = commentViewModel.ParentPostId
        };
        _commentService.UpdateComment(comment);

        return NoContent();
    }

    // DELETE api/<CommentController>/5
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
