using webapi.Models;

namespace webapi.DTOs;

public class PostDTO
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime DatePosted { get; set; }
    public string AuthorUserId { get; set; }


    public PostDTO(Post post)
    {
        Id = post.Id;
        Title = post.Title;
        Description = post.Description;
        DatePosted = post.DatePosted;
        AuthorUserId = post.User.Id;
    }
}

public class PostCollectionDTO
{
    public IEnumerable<PostDTO> Posts { get; set; }

    public PostCollectionDTO(ICollection<Post> posts)
    {
        Posts = posts.Select(post => new PostDTO(post));
    }
}
