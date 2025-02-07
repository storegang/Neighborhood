using webapi.Models;

namespace webapi.DTOs;

public class LikeDTO
{
    public string UserId { get; set; }
}

public class LikesDTO
{
    public IEnumerable<LikeDTO> Likes { get; set; }
}
