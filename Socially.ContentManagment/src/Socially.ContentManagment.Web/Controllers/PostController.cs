using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Socially.ContentManagment.Web.Extensions;
using Socially.SharedKernel.Config.JWT;
using Socially.ContentManagment.UseCases.Comments.Common.DTOs;
using Socially.ContentManagment.UseCases.Comments.Create;
using Socially.ContentManagment.UseCases.Comments.Delete;
using Socially.ContentManagment.UseCases.Comments.Update;
using Socially.ContentManagment.UseCases.Likes.Common.DTOs;
using Socially.ContentManagment.UseCases.Likes.Create;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;
using Socially.ContentManagment.UseCases.Posts.Create;
using Socially.ContentManagment.UseCases.Posts.Delete;
using Socially.ContentManagment.UseCases.Posts.Get;
using Socially.ContentManagment.UseCases.Posts.Update;
using Socially.ContentManagment.UseCases.Shares.Common.DTOs;
using Socially.ContentManagment.UseCases.Shares.Create;

namespace Socially.ContentManagment.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
  private readonly IMediator _mediator;

  public PostController(IMediator mediator)
  {
    _mediator = mediator;
  }
  [HttpGet]
  public async Task<ActionResult<PostDto>> Get(Guid id)
  {
    var query = new GetPostQuery(id);
    var result = await _mediator.Send(query);
    if(result.IsSuccess)
    {
      return Ok(result.Value);
    }
    return BadRequest(result.ToProblemDetails());
  }
  [HttpGet]
  public async Task<ActionResult<PostDto[]>> GetPosts(Guid id)
  {
    var query = new GetPostsQuery(id);
    var result = await _mediator.Send(query);
    if (result.IsSuccess)
    {
      return result.Value;
    }
    return BadRequest(result.ToProblemDetails());

  }
  [HttpDelete]
  [Authorize]
  public async Task<ActionResult> Delete(Guid id) 
  {
    var userId = Guid.Parse(User.Identity?.Name!);
    var command = new DeletePostCommand(id, userId);
    var result = await _mediator.Send(command);
    if(result.IsSuccess)
    {
      return Ok(result.Value);
    }
    return BadRequest(result.ToProblemDetails());
  }

  [HttpPost]
  [Authorize]
  public async Task<ActionResult<PostDto>> CreatePost(CreatePostDto createPostDto)
  {
    var userId = Guid.Parse(User.Identity?.Name!);
    var command = new CreatePostCommand(createPostDto, userId);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      return Ok(result.Value);
    }
    return BadRequest(result.ToProblemDetails());
  }

  [HttpPatch]
  [Authorize]
  public async Task<ActionResult<PostDto>> UpdatePost(UpdatePostDto updatePostDto)
  {
    var userId = Guid.Parse(User.Identity?.Name!);
    var command = new UpdatePostCommand(updatePostDto, userId);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      return Ok(result.Value);
    }
    return BadRequest(result.ToProblemDetails());
  }

  [HttpPost]
  [Authorize]
  public async Task<ActionResult> LikePost(ToggleLikeDto toggleLikeDto)
  {
    var userId = Guid.Parse(User.Identity?.Name!);
    var command = new ToggleLikeCommand(toggleLikeDto, userId);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      return Ok(result.Value);
    }
    return BadRequest(result.ToProblemDetails());
  }

  [HttpPost]
  [Authorize]
  public async Task<ActionResult<SharePostDto>> SharePost(SharePostDto sharePostDto)
  {
    var userId = Guid.Parse(User.Identity?.Name!);
    var command = new SharePostCommand(sharePostDto, userId);
    var result = await _mediator.Send(command);
    if(result.IsSuccess)
    {
      return Ok(result.Value);
    }
    return BadRequest(result.ToProblemDetails());
  }
  [HttpPost]
  [Authorize]
  public async Task<ActionResult> Comment(CreateCommentDto createCommentDto)
  {
    var userId = Guid.Parse(User.Identity?.Name!);
    var command = new CreateCommentCommand(createCommentDto, userId);
    var result = await _mediator.Send(command);
    if(result.IsSuccess)
    {
      return Ok(result.Value);
    }
    return BadRequest(result.ToProblemDetails());
  }
  [HttpPatch]
  [Authorize]
  public async Task<ActionResult> UpdateComment (UpdateCommentDto updateCommentDto)
  {
    var userId = Guid.Parse(User.Identity?.Name!);
    var command = new UpdateCommentCommand(updateCommentDto, userId);
    var result = await _mediator.Send(command);
    if(result.IsSuccess)
    {
      return Ok(result.Value);
    }  
    return BadRequest(result.ToProblemDetails());
  }

  [HttpDelete]
  [Authorize]
  public async Task<ActionResult> DeleteComment(Guid id)
  {
    var userId = Guid.Parse(User.Identity?.Name!);
    var command = new DeleteCommentCommand(id, userId);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      return Ok(result.Value);
    }
    return BadRequest(result.ToProblemDetails());
  }
}
