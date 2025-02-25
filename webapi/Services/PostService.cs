using Microsoft.AspNetCore.Mvc;
using webapi.DTOs;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface IPostService
{
    ICollection<Post> GetAllPosts();
    Post GetPostById(string id);
    Post GetPostByIdWithChildren(string id);
    void CreatePost(Post post);
    void UpdatePost(Post post);
    void DeletePost(string id);
    bool CheckIfCurrentUserLiked(Post post, ControllerBase user);
}

public class PostService(IPostRepository postRepository) : IPostService
{
    private readonly IPostRepository _postRepository = postRepository;

    public ICollection<Post> GetAllPosts()
    {
        return _postRepository.GetAll();
    }

    public Post GetPostById(string id)
    {
        return _postRepository.GetById(id);
    }

    public Post GetPostByIdWithChildren(string id)
    {
        return _postRepository.GetByIdWithChildren(id);
    }

    public void CreatePost(Post post)
    {
        _postRepository.Add(post);
    }

    public void UpdatePost(Post post)
    {
        _postRepository.Update(post);
    }

    public void DeletePost(string id)
    {
        Post post = _postRepository.GetById(id);

        if (post != null)
        {
            _postRepository.Delete(post);
        }
    }


    public bool CheckIfCurrentUserLiked(Post post, ControllerBase user)
    {
        if (post.LikedByUserID.Contains(user.User.Claims.First(c => c.Type.Equals("user_id"))?.Value))
        {
            return true;
        }
        return false;
    }
}
