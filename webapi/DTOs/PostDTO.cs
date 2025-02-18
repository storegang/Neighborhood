using Microsoft.Extensions.Hosting;
using webapi.Models;

namespace webapi.DTOs;

public class PostDTO
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DatePosted { get; set; }
    public DateTime? DateLastEdited { get; set; }

    public string AuthorUserId { get; set; }
    public string CategoryId { get; set; }
    public IEnumerable<string>? ImageUrls { get; set; }
    public IEnumerable<string>? LikedByUserId { get; set; }

    public PostDTO(){}

    public PostDTO(Post post)
    {
        Id = post.Id;
        Title = post.Title;
        Description = post.Description;
        DatePosted = post.DatePosted;
        DateLastEdited = post.DateLastEdited;
        AuthorUserId = post.User.Id;
        CategoryId = post.CategoryId;
        ImageUrls = post.Images;
        LikedByUserId = post.LikedByUserID;
    }

    public PostDTO(string id, string title, string? description, DateTime? datePosted, DateTime? dateLastEdited, string authorUserId, string categoryId, IEnumerable<string> imageUrls, IEnumerable<string> likedByUserId)
    {
        Id = id;
        Title = title;
        Description = description;
        DatePosted = datePosted;
        DateLastEdited = dateLastEdited;
        AuthorUserId = authorUserId;
        CategoryId = categoryId;
        ImageUrls = imageUrls;
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
