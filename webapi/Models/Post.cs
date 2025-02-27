using webapi.Interfaces;

namespace webapi.Models;

public class Post : BaseEntity, ILikeable
{
    // INHERITS: public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime DatePosted { get; set; }
    public DateTime? DateLastEdited { get; set; }

    public User User { get; set; }
    public string CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<string>? Images { get; set; } = new List<string>();
    public ICollection<string>? LikedByUserID { get; set; } = new List<string>();

    // TODO: Add image urls to POST

    // TODO: Add events and polls
}
