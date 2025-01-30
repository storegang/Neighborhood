using webapi.Models;

namespace webapi.ViewModels;

public class LikeViewModel
{
    public string Id { get; set; }
    public User User { get; set; }
    public Post Post { get; set; }
}
