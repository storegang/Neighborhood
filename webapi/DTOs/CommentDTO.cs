using webapi.Models;

namespace webapi.DTOs;

public class CommentDTO
{
    public string Id { get; set; }
    public string Content { get; set; }
    public DateTime DatePosted { get; set; }
    public string AuthorUserId { get; set; }

    public CommentDTO(Comment comment)
    {
        Id = comment.Id;
        Content = comment.Content;
        DatePosted = comment.DatePosted;
        AuthorUserId = comment.User.Id;
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
