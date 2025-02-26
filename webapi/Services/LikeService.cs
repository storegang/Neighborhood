using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface ILikeService
{
    bool LikePost(Post post, ControllerBase _controllerBase);
    bool LikeComment(Comment comment, ControllerBase _controllerBase);
}

public class LikeService : ILikeService
{

    public bool LikePost(Post post, ControllerBase _controllerBase)
    {
        if (post.LikedByUserID.Contains(_controllerBase.User.Claims.First(c => c.Type.Equals("user_id"))?.Value) && post.LikedByUserID != null)
        {
            return true;
        }
        return false;
    }

    public bool LikeComment(Comment comment, ControllerBase _controllerBase)
    {
        if (comment.LikedByUserID.Contains(_controllerBase.User.Claims.First(c => c.Type.Equals("user_id"))?.Value) && comment.LikedByUserID != null)
        {
            return true;
        }
        return false;
    }
}
