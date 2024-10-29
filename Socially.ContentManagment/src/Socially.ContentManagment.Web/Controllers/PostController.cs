using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;

namespace Socially.ContentManagment.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
  [HttpGet]
  public Task<ActionResult<PostDto>> Get(Guid id)
  {

  }
  [HttpGet]
  public Task<ActionResult<PostDto[]>> GetPosts(Guid id)
  {

  }
  [HttpDelete]
  public Task<ActionResult> Delete(Guid id) 
  { 
  }

  [HttpPost]
  public Task<ActionResult<PostDto>> CreatePost(CreatePostDto createPostDto)
  {

  }

  [HttpPatch]
  public Task<ActionResult<PostDto>> UpdatePost(UpdatePostDto updatePostDto)
  {

  }

  [HttpPost]
  public Task<ActionResult> LikePost(Guid id)
  {
  }

  [HttpPost]
  public Task<ActionResult<SharePostDto>> SharePost(Guid id)
  {
  }
  [HttpPost]
  public Task<ActionResult> Comment(CreateCommentDto createCommentDto)
  {
  }
  [HttpPatch]
  public Task<ActionResult> UpdateComment (UpdateCommentDto updateCommentDto)
  {

  }

  [HttpDelete]
  public Task<ActionResult> DeleteComment(Guid id)
  {
  }
}
