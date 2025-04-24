using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.DTOs;
using Microsoft.AspNetCore.Authorization;
using webapi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using webapi.Identity;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CommentController(IBaseService<Comment> commentService, IBaseService<Post> postService, ILikeService<Comment> likeService, UserManager<User> userManager) : ControllerBase
{
    private readonly IBaseService<Comment> _commentService = commentService;
    private readonly IBaseService<Post> _postService = postService;
    private readonly ILikeService<Comment> _likeService = likeService;
    private readonly UserManager<User> _userManager = userManager;

    // GET: api/<CommentController>
    [HttpGet]
    public async Task<ActionResult<ServerCommentCollectionDTO>> GetAll()
    {
        ICollection<Comment> commentCollection = await _commentService.GetAll([query => query.Include(c => c.User)]);
        
        ServerCommentCollectionDTO commentDataCollection = new(commentCollection);

        Comment[] comments = new Comment[commentCollection.Count()];
        comments = commentCollection.ToArray();

        ServerCommentDTO[] commentDTOs = new ServerCommentDTO[commentDataCollection.Comments.Count()];
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
    public async Task<ActionResult<ServerCommentDTO>> GetById(string id)
    {
        Comment? comment = await _commentService.GetById(id);
        
        if (comment == null)
        {
            return NotFound("Comment not found.");
        }

        ServerCommentDTO commentData = new(comment);

        commentData.LikedByCurrentUser = await _likeService.IsLiked(comment.LikedByUserID, User.Claims.First(c => c.Type.Equals("user_id"))?.Value);

        return Ok(commentData);
    }

    // GET api/<CommentController>/AllFromPost={postId}
    [HttpGet("AllFromPost={postId}")]
    public async Task<ActionResult<ServerCommentCollectionDTO>> GetCommentsByPostId(string postId)
    {
        Post? post = await _postService.GetById(postId, [query => query.Include(p => p.Comments).ThenInclude(c => c.User)]);

        if (post == null || post.Comments == null)
        {
            return NotFound("Post not found.");
        }
        ICollection<Comment>? commentCollection = post.Comments;

        ServerCommentCollectionDTO commentDataCollection = new(commentCollection);

        ICollection<Comment> comments = new Comment[commentCollection.Count()];
        comments = commentCollection.ToArray();

        IEnumerable<ServerCommentDTO> commentDTOs = new ServerCommentDTO[commentDataCollection.Comments.Count()];
        commentDTOs = commentDataCollection.Comments.ToArray();


        foreach (ServerCommentDTO commentData in commentDTOs)
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
    public async Task<ActionResult<ServerCommentCollectionDTO>> GetSomeCommentsByPostId(string postId, string page, string size = "5")
    {
        if (!int.TryParse(page, out _) || !int.TryParse(size, out _))
        {
            return BadRequest("Could not parse page or size.");
        }

        Post? post = await _postService.GetById(postId, 
            [query => query.Include(p => p.Comments
                .Skip(int.Parse(page) * int.Parse(size))
                .Take(int.Parse(size)))
                .ThenInclude(c => c.User)]);

        if (post == null)
        {
            return NotFound("Post not found.");
        }

        ICollection<Comment> commentCollection = post.Comments;

        ServerCommentCollectionDTO commentDataCollection = new(commentCollection);

        ICollection<Comment> comments = new Comment[commentCollection.Count()];
        comments = commentCollection.ToArray();

        IEnumerable<ServerCommentDTO> commentDTOs = new ServerCommentDTO[commentDataCollection.Comments.Count()];
        commentDTOs = commentDataCollection.Comments.ToArray();


        foreach (ServerCommentDTO commentData in commentDTOs)
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
    public async Task<ActionResult<ClientCommentDTO>> Create(ClientCommentDTO commentData)
    {
        Post? post = await _postService.GetById(commentData.ParentPostId, [query => query.Include(c => c.User)]);

        if (post == null)
        {
            return NotFound("Post not found.");
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
        while (await _commentService.GetById(newGuid) != null);

        Comment comment = new()
        {
            Id = newGuid,
            Content = commentData.Content,
            DatePosted = DateTime.UtcNow,
            User = user,
            ParentPostId = commentData.ParentPostId,
            ImageUrl = commentData.ImageUrl
        };
        await _commentService.Create(comment);

        post.Comments.Add(comment);
        await _postService.Update(post);

        return CreatedAtAction(nameof(GetById), new { id = newGuid }, commentData);
    }

    // PUT api/<CommentController>/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, ClientCommentDTO commentData)
    {
        Comment? existingComment = await _commentService.GetById(id, [query => query.Include(c => c.User)]);
        if (existingComment == null)
        {
            return NotFound("Comment not found.");
        }

        string claimsId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value ?? "";
        if (claimsId != existingComment.User.Id)
        {
            User? claimsUser = await _userManager.FindByIdAsync(claimsId);
            bool IsSameNeighborhood = claimsUser?.NeighborhoodId == existingComment.User.NeighborhoodId;
            bool IsBoardMember = await _userManager.IsInRoleAsync(claimsUser, UserRoles.BoardMember);

            if (!IsSameNeighborhood && !IsBoardMember)
            {
                return Unauthorized("User is not the owner of this comment or a board member in this neighborhood.");
            }
        }

        Comment comment = new() 
        {
            Id = existingComment.Id,
            Content = commentData.Content,
            DatePosted = existingComment.DatePosted,
            DateLastEdited = DateTime.UtcNow,
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
            return NotFound("Comment not found.");
        }
        string? userId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value;
        if (userId == null)
        {
            return Unauthorized("User does not exist.");
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
            return NotFound("Comment not found.");
        }

        string claimsId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value ?? "";
        if (claimsId != existingComment.User.Id)
        {
            User? claimsUser = await _userManager.FindByIdAsync(claimsId);
            bool IsSameNeighborhood = claimsUser?.NeighborhoodId == existingComment.User.NeighborhoodId;
            bool IsBoardMember = await _userManager.IsInRoleAsync(claimsUser, UserRoles.BoardMember);

            if (!IsSameNeighborhood && !IsBoardMember)
            {
                return Unauthorized("User is not the owner of this comment or a board member in this neighborhood.");
            }
        }

        await _commentService.Delete(id);

        return NoContent();
    }
}
