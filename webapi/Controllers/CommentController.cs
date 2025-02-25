using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CommentController(IGenericService<Comment> commentService, IGenericService<User> userService, IGenericService<Post> postService, LikeService likeService) : ControllerBase
{
    private readonly IGenericService<Comment> _commentService = commentService;
    private readonly IGenericService<User> _userService = userService;
    private readonly IGenericService<Post> _postService = postService;
    private readonly LikeService _likeService = likeService;

    // GET: api/<CommentController>
    [HttpGet]
    public ActionResult<CommentCollectionDTO> GetAll()
    {
        ICollection<Comment> commentCollection = _commentService.GetAll([c => c.User]);
        CommentCollectionDTO commentDataCollection = new CommentCollectionDTO(commentCollection);

        if (commentCollection == null)
        {
            return NotFound();
        }

        Comment[] comments = new Comment[commentCollection.Count()];
        comments = commentCollection.ToArray();

        CommentDTO[] commentDTOs = new CommentDTO[commentDataCollection.Comments.Count()];
        commentDTOs = commentDataCollection.Comments.ToArray();

        for (int i = 0; i < commentCollection.Count; i++)
        {
            if (comments[i].Id == commentDTOs[i].Id)
            {
                commentDTOs[i].LikedByCurrentUser = _likeService.LikeComment(comments[i], this);
            }
        }

        commentDataCollection.Comments = commentDTOs;


        return Ok(commentDataCollection);
    }

    // GET api/<CommentController>/{id}
    [HttpGet("{id}")]
    public ActionResult<CommentDTO> GetById(string id)
    {
        var comment = _commentService.GetById(id);
        
        if (comment == null)
        {
            return NotFound();
        }

        var commentData = new CommentDTO(comment);

        commentData.LikedByCurrentUser = _likeService.LikeComment(comment, this);

        return Ok(commentData);
    }

    // GET api/<CommentController>/AllFromPost={postId}
    [HttpGet("AllFromPost={postId}")]
    public ActionResult<CommentCollectionDTO> GetCommentsByPostId(string postId)
    {
        var post = _postService.GetById(postId, [c => c.User, c => c.Comments]);
        var commentCollection = post.Comments;

        if (commentCollection == null)
        {
            return NotFound();
        }

        var commentDataCollection = new CommentCollectionDTO(commentCollection);

        Comment[] comments = new Comment[commentCollection.Count()];
        comments = commentCollection.ToArray();

        CommentDTO[] commentDTOs = new CommentDTO[commentDataCollection.Comments.Count()];
        commentDTOs = commentDataCollection.Comments.ToArray();

        for (int i = 0; i < commentCollection.Count; i++)
        {
            if (comments[i].Id == commentDTOs[i].Id)
            {
                commentDTOs[i].LikedByCurrentUser = _likeService.LikeComment(comments[i], this);
            }
        }

        commentDataCollection.Comments = commentDTOs;

        return Ok(commentDataCollection);
    }

    // GET api/<CommentController>/FromPost={postId}&Page={page}&Size={size}
    // GET api/<CommentController>/FromPost={postId}&Page={page}
    [HttpGet("FromPost={postId}&Page={page}")]
    public ActionResult<CommentCollectionDTO> GetSomeCommentsByPostId(string postId, string page, string size = "5")
    {
        var post = _postService.GetById(postId, [c => c.User, c => c.Comments]);


        if (post == null || post.Comments == null)
        {
            return NotFound();
        }

        if (!int.TryParse(page, out _) || !int.TryParse(size, out _))
        {
            return BadRequest();
        }

        var commentCollection = post.Comments
        .Skip((int.Parse(page) - 1) * int.Parse(size))
        .Take(int.Parse(size))
        .ToList();

        var commentDataCollection = new CommentCollectionDTO(commentCollection);

        Comment[] comments = new Comment[commentCollection.Count()];
        comments = commentCollection.ToArray();

        CommentDTO[] commentDTOs = new CommentDTO[commentDataCollection.Comments.Count()];
        commentDTOs = commentDataCollection.Comments.ToArray();

        for (int i = 0; i < commentCollection.Count; i++)
        {
            if (comments[i].Id == commentDTOs[i].Id)
            {
                commentDTOs[i].LikedByCurrentUser = _likeService.LikeComment(comments[i], this);
            }
        }

        commentDataCollection.Comments = commentDTOs;

        return Ok(commentDataCollection);
    }

    // TODO: Add OrderBy 


    // POST api/<CommentController>
    [HttpPost]
    public ActionResult<CommentDTO> Create(CommentDTO commentData)
    {
        var post = _postService.GetById(commentData.ParentPostId, [c => c.User]);

        if (post == null)
        {
            return NotFound();
        }

        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_commentService.GetById(newGuid) != null);

        commentData.Id = newGuid;
        var comment = new Comment
        {
            Id = commentData.Id,
            Content = commentData.Content,
            DatePosted = DateTime.Now,
            User = _userService.GetById(commentData.AuthorUserId),
            ParentPostId = commentData.ParentPostId,
            ImageUrl = commentData.ImageUrl
        };
        _commentService.Create(comment);

        post.Comments.Add(comment);
        _postService.Update(post);

        return CreatedAtAction(nameof(GetById), new { id = commentData.Id }, commentData);
    }

    // PUT api/<CommentController>/{id}
    [HttpPut("{id}")]
    public IActionResult Update(string id, CommentDTO commentData)
    {
        var existingComment = _commentService.GetById(id, [c => c.User]);
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
        _commentService.Update(comment);

        return NoContent();
    }

    // PUT api/<CommentController>/Likes/{commentId}
    [HttpPut("Likes/{commentId}")]
    public IActionResult Like(string commentId)
    {
        var comment = _commentService.GetById(commentId, [c => c.User]);
        if (comment == null)
        {
            return NotFound();
        }
        var userId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value;

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
        _commentService.Update(comment);
        return NoContent();
    }

    // DELETE api/<CommentController>/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existingComment = _commentService.GetById(id);
        if (existingComment == null)
        {
            return NotFound();
        }

        _commentService.Delete(id);

        return NoContent();
    }
}
