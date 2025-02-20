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
    public User? AuthorUser { get; set; }
    public string CategoryId { get; set; }
    public IEnumerable<string>? ImageUrls { get; set; }
    public int? CommentCount { get; set; }
    public int? LikedByUserCount { get; set; }
    public bool? LikedByCurrentUser { get; set; }

    public PostDTO(){}

    public PostDTO(Post post)
    {
        Id = post.Id;
        Title = post.Title;
        Description = post.Description;
        DatePosted = post.DatePosted;
        DateLastEdited = post.DateLastEdited;
        AuthorUserId = post.User.Id;
        AuthorUser = post.User;
        CategoryId = post.CategoryId;
        ImageUrls = post.Images;
        CommentCount = post.Comments.Count();
        LikedByUserCount = post.LikedByUserID?.Count();
    }

    public PostDTO(string id, string title, string? description, DateTime? datePosted, DateTime? dateLastEdited, 
        User? authorUser, string categoryId, IEnumerable<string>? imageUrls, int? commentCount, int? likedByUserCount, bool likedByCurrentUser)
    {
        Id = id;
        Title = title;
        Description = description;
        DatePosted = datePosted;
        DateLastEdited = dateLastEdited;
        AuthorUserId = authorUser.Id;
        AuthorUser = authorUser;
        CategoryId = categoryId;
        ImageUrls = imageUrls;
        CommentCount = commentCount;
        LikedByUserCount = likedByUserCount;
        LikedByCurrentUser = likedByCurrentUser;
    }

    public PostDTO(string id, string title, string? description, DateTime? datePosted, DateTime? dateLastEdited, string authorUserId,
        User? authorUser, string categoryId, IEnumerable<string>? imageUrls, int? commentCount, int? likedByUserCount, bool likedByCurrentUser)
    {
        Id = id;
        Title = title;
        Description = description;
        DatePosted = datePosted;
        DateLastEdited = dateLastEdited;
        AuthorUserId = authorUserId;
        AuthorUser = authorUser;
        CategoryId = categoryId;
        ImageUrls = imageUrls;
        CommentCount = commentCount;
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
