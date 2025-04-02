using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Interfaces;

public interface ILikeService<T> where T : ILikeable
{
    Task<bool> IsLiked(ICollection<string>? likeable, string? userId);
}

public interface ILikeable
{
    ICollection<string>? LikedByUserID { get; set; }
}
