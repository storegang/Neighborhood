namespace webapi.Models;

public class Comment
{
    public string Id { get; set; }
    public string Content { get; set; }

    public DateTime DatePosted { get; set; }

    public User User { get; set; }
    public Post ParentPost { get; set; }

    public ICollection<string>? LikedByUserID { get; set; }
}
