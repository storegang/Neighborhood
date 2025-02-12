namespace webapi.Models;

public class Post
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime DatePosted { get; set; }

    public User User { get; set; }
    public string CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<string>? Images { get; set; }
    public ICollection<string>? LikedByUserID { get; set; }

    // TODO: Add image urls to POST

    // TODO: Add events and polls
}
