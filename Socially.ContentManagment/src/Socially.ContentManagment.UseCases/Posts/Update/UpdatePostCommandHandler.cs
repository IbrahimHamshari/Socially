using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.ContentManagment.Core.PostAggregate;
using Socially.ContentManagment.Core.PostAggregate.Errors;
using Socially.ContentManagment.UseCases.Posts.Common.DTOs;

namespace Socially.ContentManagment.UseCases.Posts.Update;
public class UpdatePostCommandHandler(IRepository<Post> _repository) : ICommandHandler<UpdatePostCommand, Result<PostDto>>
{
  public async Task<Result<PostDto>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
  {
    Guid id = request.updatePostDto.Id;
    var updatedPost = request.updatePostDto;
    var post = await _repository.GetByIdAsync(id);
    if (post == null)
    {
      return PostErrors.NotFound(id);
    }
    if(updatedPost.Content != null)
    {
      post.UpdateContent(updatedPost.Content);
    }
    if (updatedPost.MediaURL != null)
    {
      post.UpdateMediaURL(updatedPost.MediaURL);
    }
    if (updatedPost.Privacy != null)
    {
      Privacy privacy = Privacy.Public;
      switch(updatedPost.Privacy)
      {
        case 0: privacy = Privacy.Public; break;
        case 1: privacy = Privacy.Private; break;
        case 2: privacy = Privacy.Friends; break;
      }
      post.UpdatePrivacy(privacy);
    }

    await _repository.SaveChangesAsync();
    return Result.Success(new PostDto {Id = post.Id, Content = post.Content, MediaURL = post.MediaURL, Privacy = post.Privacy, UserId = post.UserID });

  }
}
