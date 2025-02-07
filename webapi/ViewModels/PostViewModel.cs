using webapi.Models;

namespace webapi.ViewModels;

public class PostViewModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime DatePosted { get; set; }

    public UserViewModel User { get; set; }

    public string CategoryId { get; set; }
    public ICollection<CommentViewModel>? Comments { get; set; }
    public ICollection<string>? Images { get; set; }
    public ICollection<string>? LikedByUserId { get; set; }

    // TODO: Add image urls to POST

    // TODO: Add events and polls
}