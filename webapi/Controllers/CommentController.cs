using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.Models;
using webapi.ViewModels;
using AutoMapper;

namespace webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController(CommentService commentService, IMapper mapper) : ControllerBase
{
    private readonly CommentService _commentService = commentService;
    private readonly IMapper _mapper = mapper;

    // GET: api/<CommentController>
    [HttpGet]
    public ActionResult<IEnumerable<CommentViewModel>> GetAll()
    {
        var comments = _commentService.GetAllComments();
        var commentViewModels = _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentViewModel>>(comments);
        return Ok(commentViewModels);
    }

    // GET api/<CommentController>/5
    [HttpGet("{id}")]
    public ActionResult<CommentViewModel> GetById(string id)
    {
        var comment = _commentService.GetCommentById(id);
        
        if (comment == null)
        {
            return NotFound();
        }

        var commentViewModel = _mapper.Map<Comment,  CommentViewModel>(comment);
        return Ok(commentViewModel);
    }

    // POST api/<CommentController>
    [HttpPost]
    public ActionResult<CommentViewModel> Create(CommentViewModel commentViewModel)
    {
        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (_commentService.GetCommentById(newGuid) != null);

        commentViewModel.Id = newGuid;
        var comment = _mapper.Map<CommentViewModel, Comment>(commentViewModel);
        _commentService.CreateComment(comment);

        return CreatedAtAction(nameof(GetById), new { id = commentViewModel.Id }, commentViewModel);
    }

    // PUT api/<CommentController>/5
    [HttpPut("{id}")]
    public IActionResult Update(string id, CommentViewModel commentViewModel)
    {
        var existingComment = _commentService.GetCommentById(id);
        if (existingComment == null)
        {
            return NotFound();
        }

        commentViewModel.Id = id;
        var comment = _mapper.Map<CommentViewModel, Comment>(commentViewModel);
        _commentService.UpdateComment(comment);

        return NoContent();
    }

    // DELETE api/<CommentController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existingComment = _commentService.GetCommentById(id);
        if (existingComment == null)
        {
            return NotFound();
        }

        _commentService.DeleteComment(id);

        return NoContent();
    }
}
