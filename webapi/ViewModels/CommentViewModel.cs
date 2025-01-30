using webapi.Models;

namespace webapi.ViewModels;

public class CommentViewModel
{
    public string Id { get; set; }
    public string Content { get; set; }

    public User User { get; set; }
    public Post ParentPost { get; set; }
}
