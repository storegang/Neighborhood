
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using webapi.DTOs;
using webapi.Interfaces;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface ICommentService : IBaseService<Comment>, ILikeService<Comment>
{
}

public class CommentService(IGenericRepository<Comment> repository) : BaseService<Comment>(repository), ICommentService
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
