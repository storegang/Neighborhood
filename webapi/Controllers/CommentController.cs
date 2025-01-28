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
    public ActionResult<CommentViewModel> GetById(int id)
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
        var comment = _mapper.Map<CommentViewModel, Comment>(commentViewModel);
        _commentService.CreateComment(comment);

        commentViewModel.Id = comment.Id;

        return CreatedAtAction(nameof(GetById), new { id = commentViewModel.Id }, commentViewModel);
    }

    // PUT api/<CommentController>/5
    [HttpPut("{id}")]
    public IActionResult Update(int id, CommentViewModel commentViewModel)
    {
        if (id != int.Parse(commentViewModel.Id))
        {
            return BadRequest();
        }

        var existingComment = _commentService.GetCommentById(id);
        if (existingComment == null)
        {
            return NotFound();
        }

        var comment = _mapper.Map<CommentViewModel, Comment>(commentViewModel);
        _commentService.UpdateComment(comment);

        return NoContent();
    }

    // DELETE api/<CommentController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
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
