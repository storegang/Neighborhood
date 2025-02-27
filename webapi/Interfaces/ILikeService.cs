using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Interfaces;

public interface ILikeService<T> where T : ILikeable
{
    bool Like(ICollection<string>? likeable, string? userId);
}

public interface ILikeable
{
    ICollection<string>? LikedByUserID { get; set; }
}
