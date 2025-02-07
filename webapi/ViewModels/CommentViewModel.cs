using webapi.Models;

namespace webapi.ViewModels;

public class CommentViewModel
{
    public string Id { get; set; }
    public string Content { get; set; }

    public DateTime DatePosted { get; set; }

    public UserViewModel User { get; set; }
    public string ParentPostId { get; set; }

    public ICollection<string>? LikedByUserID { get; set; }
}
