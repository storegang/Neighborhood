
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
    public bool Like(ICollection<string>? likeable, string? userId)
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
