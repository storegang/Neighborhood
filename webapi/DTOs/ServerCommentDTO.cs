using webapi.Models;

namespace webapi.DTOs;

public class ServerCommentDTO
{
    public string Id { get; set; }
    public string Content { get; set; }
    public DateTime? DatePosted { get; set; }
    public DateTime? DateLastEdited { get; set; }
    public string AuthorUserId { get; set; }
    public UserDTO? AuthorUser { get; set; }
    public string ParentPostId { get; set; }

    public string? ImageUrl { get; set; }
    public int? LikedByUserCount { get; set; }
    public bool? LikedByCurrentUser { get; set; }

    public ServerCommentDTO(){}

    public ServerCommentDTO(Comment comment)
    {
        Id = comment.Id;
        Content = comment.Content;
        DatePosted = comment.DatePosted;
        DateLastEdited = comment.DateLastEdited;
        AuthorUserId = comment.User.Id; // TODO: Errors if user that made the comment doesn't exist
        AuthorUser = new UserDTO(comment.User);
        ParentPostId = comment.ParentPostId;
        ImageUrl = comment.ImageUrl;
        LikedByUserCount = comment.LikedByUserID?.Count();
    }

    public ServerCommentDTO(string id, string content, DateTime? datePosted, DateTime? dateLastEdited, 
        string authorUserId, User? authorUser, string parentPostId, string? imageUrl, ICollection<string>? likedByUserIds)
    {
        Id = id;
        Content = content;
        DatePosted = datePosted;
        DateLastEdited = dateLastEdited;
        AuthorUserId = authorUserId;
        AuthorUser = new UserDTO(authorUser);
        ParentPostId = parentPostId;
        ImageUrl = imageUrl;
        LikedByUserCount = likedByUserIds?.Count();
    }
}

public class ServerCommentCollectionDTO
{
    public IEnumerable<ServerCommentDTO> Comments { get; set; }

    public ServerCommentCollectionDTO(ICollection<Comment> comments)
    {
        Comments = comments.Select(comment => new ServerCommentDTO(comment));
    }
}
