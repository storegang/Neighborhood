using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public class LikeService(ILikeRepository likeRepository)
{
    private readonly ILikeRepository _likeRepository = likeRepository;

    public ICollection<Like> GetAllLikes()
    {
        return _likeRepository.GetAll();
    }

    public Like GetLikeById(string id)
    {
        return _likeRepository.GetById(id);
    }

    public void CreateLike(Like like)
    {
        _likeRepository.Add(like);
    }

    public void UpdateLike(Like like)
    {
        _likeRepository.Update(like);
    }

    public void DeleteLike(string id)
    {
        Like like = _likeRepository.GetById(id);

        if (like != null)
        {
            _likeRepository.Delete(like);
        }
    }
}
