using webapi.Models;

namespace webapi.DTOs;

public class LikeDTO
{
    public string UserId { get; set; }

    public LikeDTO(Like like)
    {
        UserId = like.User.Id;
    }
}

public class LikesCollectionDTO
{
    public IEnumerable<LikeDTO> Likes { get; set; }

    public LikesCollectionDTO(ICollection<Like> likes)
    {
        Likes = likes.Select(like => new LikeDTO(like));
    }
}
