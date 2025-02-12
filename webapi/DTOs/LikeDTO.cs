using webapi.Models;

namespace webapi.DTOs;

public class LikeDTO
{
    public string UserId { get; set; }

    public LikeDTO(Like like)
    {
        UserId = like.User.Id;
    }

    public LikeDTO(string userId)
    {
        UserId = userId;
    }
}

public class LikeCollectionDTO
{
    public IEnumerable<LikeDTO> Likes { get; set; }

    public LikeCollectionDTO(ICollection<Like> likes)
    {
        Likes = likes.Select(like => new LikeDTO(like));
    }
}
