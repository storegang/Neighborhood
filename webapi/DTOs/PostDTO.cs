using webapi.Models;

namespace webapi.DTOs;

public class PostDTO
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime DatePosted { get; set; }
    public string AuthorUserId { get; set; }
}

public class PostsDTO
{
    public IEnumerable<PostDTO> Posts { get; set; }
}
