using webapi.DTOs;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public class PostService(IPostRepository postRepository)
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
}
