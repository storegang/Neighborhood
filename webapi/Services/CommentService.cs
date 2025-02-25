
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using webapi.DTOs;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public interface ICommentService
{
    ICollection<Comment> GetAllComments();
    Comment GetCommentById(string id);
    void CreateComment(Comment comment);
    void UpdateComment(Comment comment);
    void DeleteComment(string id);
    bool CheckIfCurrentUserLiked(Comment comment, ControllerBase user);

}

public class CommentService(ICommentRepository commentRepository) : ICommentService
{
    private readonly ICommentRepository _commentRepository = commentRepository;

    public ICollection<Comment> GetAllComments()
    {
        return _commentRepository.GetAll();
    }

    public Comment GetCommentById(string id)
    {
        return _commentRepository.GetById(id);
    }

    public void CreateComment(Comment comment)
    {
        _commentRepository.Add(comment);
    }

    public void UpdateComment(Comment comment)
    {
        _commentRepository.Update(comment);
    }

    public void DeleteComment(string id)
    {
        Comment comment = _commentRepository.GetById(id);

        if (comment != null)
        {
            _commentRepository.Delete(comment);
        }
    }


    public bool CheckIfCurrentUserLiked(Comment comment, ControllerBase user)
    {
        if (comment.LikedByUserID.Contains(user.User.Claims.First(c => c.Type.Equals("user_id"))?.Value))
        {
            return true;
        }
        return false;
    }
}
