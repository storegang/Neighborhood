using Microsoft.Extensions.Hosting;
using webapi.Models;

namespace webapi.DTOs;

public class ServerPostDTO
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DatePosted { get; set; }
    public DateTime? DateLastEdited { get; set; }

    public string AuthorUserId { get; set; }
    public UserDTO? AuthorUser { get; set; }
    public string CategoryId { get; set; }
    public IEnumerable<string>? ImageUrls { get; set; }
    public int? CommentCount { get; set; }
    public int? LikedByUserCount { get; set; }
    public bool? LikedByCurrentUser { get; set; }

    public ServerPostDTO(){}

    public ServerPostDTO(Post post)
    {
        Id = post.Id;
        Title = post.Title;
        Description = post.Description;
        DatePosted = post.DatePosted;
        DateLastEdited = post.DateLastEdited;
        AuthorUserId = post.User.Id;
        AuthorUser = new UserDTO(post.User);
        CategoryId = post.CategoryId;
        ImageUrls = post.Images;
        CommentCount = post.Comments.Count();
        LikedByUserCount = post.LikedByUserID?.Count();
    }

    public ServerPostDTO(string id, string title, string? description, DateTime? datePosted, DateTime? dateLastEdited, 
        User? authorUser, string categoryId, IEnumerable<string>? imageUrls, int? commentCount, int? likedByUserCount, bool likedByCurrentUser)
    {
        Id = id;
        Title = title;
        Description = description;
        DatePosted = datePosted;
        DateLastEdited = dateLastEdited;
        AuthorUserId = authorUser.Id;
        AuthorUser = new UserDTO(authorUser);
        CategoryId = categoryId;
        ImageUrls = imageUrls;
        CommentCount = commentCount;
        LikedByUserCount = likedByUserCount;
        LikedByCurrentUser = likedByCurrentUser;
    }

    public ServerPostDTO(string id, string title, string? description, DateTime? datePosted, DateTime? dateLastEdited, string authorUserId,
        User? authorUser, string categoryId, IEnumerable<string>? imageUrls, int? commentCount, int? likedByUserCount, bool likedByCurrentUser)
    {
        Id = id;
        Title = title;
        Description = description;
        DatePosted = datePosted;
        DateLastEdited = dateLastEdited;
        AuthorUserId = authorUserId;
        AuthorUser = new UserDTO(authorUser);
        CategoryId = categoryId;
        ImageUrls = imageUrls;
        CommentCount = commentCount;
        LikedByUserCount = likedByUserCount;
        LikedByCurrentUser = likedByCurrentUser;
    }
}

public class ServerPostCollectionDTO
{
    public IEnumerable<ServerPostDTO> Posts { get; set; }

    public ServerPostCollectionDTO(ICollection<Post> posts)
    {
        Posts = posts.Select(post => new ServerPostDTO(post));
    }
}
