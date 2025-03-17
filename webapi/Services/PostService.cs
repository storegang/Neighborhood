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
    public Task<bool> IsLiked(ICollection<string>? likeable, string? userId)
    {
        if (likeable == null || likeable.Count <= 0 || userId == null)
        {
            return Task.FromResult(false);
        }

        if (likeable.Contains(userId))
        {
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
