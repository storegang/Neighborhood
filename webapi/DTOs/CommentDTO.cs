using webapi.Models;

namespace webapi.DTOs;

public class CommentDTO
{
    public string Id { get; set; }
    public string Content { get; set; }
    public DateTime DatePosted { get; set; }
    public string AuthorUserId { get; set; }
}

public class CommentsDTO
{
    public IEnumerable<CommentDTO> Comments { get; set; }
}
