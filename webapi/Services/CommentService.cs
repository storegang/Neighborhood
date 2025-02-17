using webapi.DTOs;
using webapi.Models;
using webapi.Repositories;

namespace webapi.Services;

public class CommentService(ICommentRepository commentRepository)
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
}
