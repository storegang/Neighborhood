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

    public User AuthorUser { get; set; }
    public string CategoryId { get; set; }
    public IEnumerable<string>? ImageUrls { get; set; }
    public int? LikedByUserCount { get; set; }
    public bool LikedByCurrentUser { get; set; }

    public PostDTO(){}

    public PostDTO(Post post)
    {
        Id = post.Id;
        Title = post.Title;
        Description = post.Description;
        DatePosted = post.DatePosted;
        DateLastEdited = post.DateLastEdited;
        AuthorUser = post.User;
        CategoryId = post.CategoryId;
        ImageUrls = post.Images;
        LikedByUserCount = post.LikedByUserID?.Count();
    }

    public PostDTO(string id, string title, string? description, DateTime? datePosted, DateTime? dateLastEdited, 
        User? authorUserId, string categoryId, IEnumerable<string>? imageUrls, int? likedByUserCount, bool likedByCurrentUser)
    {
        Id = id;
        Title = title;
        Description = description;
        DatePosted = datePosted;
        DateLastEdited = dateLastEdited;
        AuthorUser = authorUserId;
        CategoryId = categoryId;
        ImageUrls = imageUrls;
        LikedByUserCount = likedByUserCount;
        LikedByCurrentUser = likedByCurrentUser;
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
