using Microsoft.AspNetCore.Mvc;
using webapi.DTOs;
using webapi.Interfaces;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface IPostService : IBaseService<Post>, ILikeService<Post>
{
}

public class PostService(IGenericRepository<Post> repository) : BaseService<Post>(repository), IPostService
{
    public bool IsLiked(ICollection<string>? likeable, string? userId)
    {
        if (likeable == null || likeable.Count <= 0 || userId == null)
        {
            return false;
        }

        if (likeable.Contains(userId))
        {
            return true;
        }
        return false;
    }
}
