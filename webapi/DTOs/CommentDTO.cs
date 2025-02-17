using webapi.Models;

namespace webapi.DTOs;

public class CommentDTO
{
    public string Id { get; set; }
    public string Content { get; set; }
    public DateTime? DatePosted { get; set; }
    public DateTime? DateLastEdited { get; set; }
    public string AuthorUserId { get; set; }
    public string ParentPostId { get; set; }

    public CommentDTO(){}

    public CommentDTO(Comment comment)
    {
        Id = comment.Id;
        Content = comment.Content;
        DatePosted = comment.DatePosted;
        DateLastEdited = comment.DateLastEdited;
        AuthorUserId = comment.User.Id;
        ParentPostId = comment.ParentPostId;
    }

    public CommentDTO(string id, string content, DateTime? datePosted, DateTime? dateLastEdited, string authorUserId, string parentPostId)
    {
        Id = id;
        Content = content;
        DatePosted = datePosted;
        DateLastEdited = dateLastEdited;
        AuthorUserId = authorUserId;
        ParentPostId = parentPostId;
    }
}

public class CommentCollectionDTO
{
    public IEnumerable<CommentDTO> Comments { get; set; }

    public CommentCollectionDTO(ICollection<Comment> comments)
    {
        Comments = comments.Select(comment => new CommentDTO(comment));
    }
}
