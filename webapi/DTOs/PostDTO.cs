using Microsoft.Extensions.Hosting;
using webapi.Models;

namespace webapi.DTOs;

public class PostDTO
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime DatePosted { get; set; }

    public string AuthorUserId { get; set; }
    public string CategoryId { get; set; }
    public IEnumerable<string>? LikedByUserId { get; set; }

    public PostDTO(){}

    public PostDTO(Post post)
    {
        Id = post.Id;
        Title = post.Title;
        Description = post.Description;
        DatePosted = post.DatePosted;
        AuthorUserId = post.User.Id;
        CategoryId = post.CategoryId;
        LikedByUserId = post.LikedByUserID;
    }

    public PostDTO(string id, string title, string? description, DateTime datePosted, string authorUserId, string categoryId, IEnumerable<string> likedByUserId)
    {
        Id = id;
        Title = title;
        Description = description;
        DatePosted = datePosted;
        AuthorUserId = authorUserId;
        CategoryId = categoryId;
        LikedByUserId = likedByUserId;
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
