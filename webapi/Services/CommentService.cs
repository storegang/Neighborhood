
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using webapi.DTOs;
using webapi.Interfaces;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface ICommentService : IGenericService<Comment>, ILikeService<Comment>
{
}

public class CommentService(IGenericService<Comment> genericService) : ICommentService
{
    private readonly IGenericService<Comment> _genericService = genericService;

    public ICollection<Comment>? GetAll(Expression<Func<Comment, object>>[]? includes = null, Expression<Func<Comment, object>>[]? thenInclude = null)
    {
        return _genericService.GetAll(includes, thenInclude);
    }

    public Comment? GetById(string id, Expression<Func<Comment, object>>[]? includes = null, Expression<Func<Comment, object>>[]? thenInclude = null)
    {
        return _genericService.GetById(id, includes, thenInclude);
    }

    public void Create(Comment comment)
    {
        _genericService.Create(comment);
    }

    public void Update(Comment comment)
    {
        _genericService.Update(comment);
    }

    public void Delete(string id)
    {
        Comment comment = _genericService.GetById(id);

        if (comment != null)
        {
            _genericService.Delete(id);
        }
    }

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
